using System;
using System.Collections.Generic;
using System.Linq;
using Animal.AnimalStates;
using Animal.Managers;
using Animal.Sensor;
using Core;
using Foods;
using Pools;
using UI;
using UI.Properties;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

namespace Animal
{
  /// <summary>
  ///   A very basic animal that searches for food.
  /// </summary>
  public abstract class AbstractAnimal : MonoBehaviour, ICanDrink, ICanEat, ITickable, IInspectable, IEatable,
    IResetable
  {
    public delegate void AgeChanged(int age);

    public delegate void ChildSpawned(AbstractAnimal child, AbstractAnimal parent);

    public delegate void Died(AbstractAnimal animal);

    public delegate void PregnancyChanged(bool isPregnant);

    public delegate void PropertiesChanged();

    public delegate void StateChanged(string state);


    public delegate void AnimalDecayed(AbstractAnimal animal);


    private const float BiggestMutationChange = 0.3f;
    private const float MutationPercentPerDay = 10f;
    private const float RunningSpeedFactor = 5f;

    /// <summary>
    ///   Scales the animal, is not correlated to actual size for the model logic.
    /// </summary>
    [SerializeField] private float VisualSizeModifier;

    [SerializeField] protected GoToMovement movement;
    [SerializeField] protected FoodManager foodManager;
    [SerializeField] protected WaterManager waterManager;
    [SerializeField] protected GameObject childPrefab;
    [SerializeField] private MatingManager matingManager;
    [SerializeField] protected ParticleSystem mouthParticles;
    [SerializeField] protected Hearing hearing;
    [SerializeField] protected Vision vision;
    [SerializeField] private AnimationManager animationManager;
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;
    [SerializeField] private int fertilityTimeInHours = 5;
    [SerializeField] private AnimalSpecies _species;
    [SerializeField] private int maxNumberOfChildren = 1;
    [SerializeField] private float pregnancyTimeInHours;
    [SerializeField] private int maximumAgeInDays = 20; //this is serialized to test for equilibrium easier TODO make private

    private float _hoursUntilPregnancy;

    private float _fleeSpeed;
    protected HealthDelegate _healthDelegate;
    private AbstractAnimal _mateTarget;
    protected NourishmentDelegate _nourishmentDelegate;
    private float _nutritionalValue;
    protected StaminaDelegate _staminaDelegate;
    private int _nourishmentMultiplier = 100;
    private StateMachine<AnimalState> _stateMachine;
    private int _hoursUntilFertile;
    public bool IsChild { get; private set; }

    private float _fullyGrownSpeed;
    private float _fullyGrownSize;
    public AgeChanged AgeChangedListeners;
    public ChildSpawned ChildSpawnedListeners;
    public Died DiedListeners;
    public AnimalDecayed DecayedListeners;
    public PregnancyChanged PregnancyChangedListeners;
    public PropertiesChanged PropertiesChangedListeners;
    public StateChanged StateChangedListeners;
    public bool IsPregnant { get; private set; }
    public bool IsRunning { get; set; }

    public float NutritionalValue
    {
      get => _nutritionalValue;
      private set
      {
        _nutritionalValue = value;
        PropertiesChangedListeners?.Invoke();
      }
    }

    public AbstractAnimal EnemyToFleeFrom { get; set; }
    public float SizeModifier { get; private set; }
    public float SpeedModifier { get; private set; }
    public IEatable FoodAboutTooEat { get; set; }
    public int AgeInDays { get; private set; }
    public bool ShouldBirth { get; private set; }
    public AbstractAnimal LastMaleMate { get; private set; }
    public bool Fertile { get; private set; }
    public Gender Gender { get; private set; }

    private float childrenSizeWhenBorn = 0.5f;

    public AnimalSpecies Species
    {
      get => _species;
      protected set => _species = value;
    }

    public bool MultipleChildren => maxNumberOfChildren > 1;
    public Water ClosestKnownWater => waterManager.ClosestKnownWater;
    public bool IsHungry => _nourishmentDelegate.IsHungry;
    public bool IsThirsty => _nourishmentDelegate.IsThirsty;
    private float Health => _healthDelegate.Health;
    private float Stamina => _staminaDelegate.Stamina;
    public bool Alive => Health > 0 && NutritionalValue >= 0.1;
    public bool Dead => !Alive;
    public bool IsCarnivore => Species == AnimalSpecies.Wolf; // TODO
    public bool IsHerbivore => Species == AnimalSpecies.Rabbit;

    /// <summary>
    ///   The amount of children that the animal has birthed.
    /// </summary>
    public int Children { get; set; }

    /// <summary>
    ///   The margin for which is the animal considers to have reached its desired position.
    /// </summary>
    public float Reach => 2f;

    /// <summary>
    ///   Whether the animal knows about a food location.
    /// </summary>
    public bool KnowsFoodLocation { get; private set; }

    /// <summary>
    ///   Whether the animal knows about a water location.
    /// </summary>
    public bool KnowsWaterLocation { get; private set; }

    /// <summary>
    ///   Returns a collection of the foods that the animal is aware of.
    /// </summary>
    public IEnumerable<FoodManager.FoodMemory> KnownFoods => foodManager.KnownFoodMemories;

    private void Awake()
    {
      _nourishmentDelegate = new NourishmentDelegate();
      _healthDelegate = new HealthDelegate();
      _staminaDelegate = new StaminaDelegate();
    }

    private void Start()
    {
      InitStateMachine();
      InitSensoryEvents();

      ResetGameObject();
    }

    private void Update()
    {
      _stateMachine.Execute();
    }

    public float GetHydration()
    {
      return _nourishmentDelegate.Hydration;
    }

    public void Drink(float hydration)
    {
      _nourishmentDelegate.Hydration += hydration;
    }

    public float GetSaturation()
    {
      return _nourishmentDelegate.Saturation;
    }

    public void SwallowEat(float saturation)
    {
      _nourishmentDelegate.Saturation += saturation;
    }

    public float Consume(float amount)
    {
      float consumedFood;

      if (NutritionalValue >= amount)
      {
        // Eat partially
        NutritionalValue -= amount;
        consumedFood = amount;
      }
      else
      {
        // Eat whole food
        consumedFood = NutritionalValue;
        NutritionalValue = 0;
      }

      return consumedFood;
    }

    public bool CanBeEaten()
    {
      return NutritionalValue > 0.1;
    }

    public IEnumerable<AbstractProperty> GetProperties()
    {
      return PropertiesFactory.Create(this);
    }

    public void ShowGizmos(bool show)
    {
      var hearingDetector = GetComponentInChildren<Hearing>();
      var visualDetector = GetComponentInChildren<Vision>();
      hearingDetector.GetComponent<Renderer>().enabled = show;
      visualDetector.GetComponent<Renderer>().enabled = show;
    }

    public void ResetGameObject()
    {
      ResetGender();
      ResetProperties();
      ResetHealthAndActivate();
      ResetStateMachine();
      ResetFertility();
    }

    public void HourTick()
    {
      _nourishmentDelegate.HourTick();
      foodManager.HourTick();
      DecreaseHealthIfStarving();
      IncreaseHealthIfSatiated();
      DecreaseStaminaIfRunning();
      IncreaseStaminaIfNotRunning();

      if (IsPregnant)
      {
        _hoursUntilPregnancy--;
        if (_hoursUntilPregnancy <= 0)
        {
          ShouldBirth = true;
          IsPregnant = false;
          PregnancyChangedListeners?.Invoke(IsPregnant);
        }
      }
      else
      {
        if (!Fertile) _hoursUntilFertile--;
        if (_hoursUntilFertile <= 0) Fertile = true;
      }
    }

    public void DayTick()
    {
      if (IsChild && AgeInDays >= fertilityTimeInHours / 24)
      {
        IsChild = false;
      }

      AgeInDays++;
      AgeChangedListeners?.Invoke(AgeInDays);
      if (IsChild)
      {
        var updateAmount = 1 / Mathf.Floor(fertilityTimeInHours / 24) * childrenSizeWhenBorn;
        SpeedModifier += _fullyGrownSpeed * updateAmount;
        SizeModifier += _fullyGrownSize * updateAmount;
        UpdateScale();
      }

      if (AgeInDays > maximumAgeInDays)
      {
        var updateAmount = 1/5;
        SpeedModifier -= SpeedModifier * updateAmount; 
        UpdateNourishmentDelegate();
      }

      Mutate();
    }

    private void ResetHealthAndActivate()
    {
      gameObject.SetActive(true);
      _healthDelegate.ResetHealth();
    }

    private void Mutate()
    {
      if (MutationPercentPerDay > Random.Range(0, 100))
      {
        SpeedModifier = Random.Range(SpeedModifier * (1 - BiggestMutationChange),
          SpeedModifier * (1 + BiggestMutationChange));

        SizeModifier = Random.Range(SizeModifier * (1 - BiggestMutationChange),
          SizeModifier * (1 + BiggestMutationChange));
        PropertiesChangedListeners?.Invoke();

        UpdateScale();
      }
    }

    private void UpdateScale()
    {
      transform.localScale = Vector3.one * (SizeModifier * VisualSizeModifier);
      UpdateNourishmentDelegate();
    }

    public bool CanEatMore()
    {
      return _nourishmentDelegate.SaturationIsFull();
    }

    public bool CanDrinkMore()
    {
      return _nourishmentDelegate.HydrationIsFull();
    }

    protected abstract void OnAnimalHeard(AbstractAnimal animal);

    protected abstract void OnEnemySeen(AbstractAnimal animal);

    protected abstract void RenderAnimalSpecificColors();

    private void ClearEnemyTarget()
    {
      EnemyToFleeFrom = null;
    }

    private void OnMateFound(AbstractAnimal animal)
    {
      var sameTypeOfAnimal = animal.Species == Species;
      var oppositeGender = animal.Gender != Gender;
      var fertile = animal.Fertile;

      if (sameTypeOfAnimal && oppositeGender && fertile &&
          (_mateTarget.DoesNotExist() || IsCloserThanPreviousMateTarget(animal)))
        _mateTarget = animal;
    }

    private bool IsCloserThanPreviousMateTarget(AbstractAnimal newTarget)
    {
      var newDistance = Vector3Util.Distance(gameObject, newTarget.gameObject);
      var oldDistance = Vector3Util.Distance(gameObject, _mateTarget.gameObject);
      return oldDistance > newDistance;
    }

    public void ClearMateTarget()
    {
      _mateTarget = null;
    }

    protected abstract List<IState<AnimalState>> GetStates(FoodManager foodManager);

    public void OnWaterLocationChanged(Water water)
    {
      KnowsWaterLocation = water != null;
    }

    /// <summary>
    ///   Gets called when the list of known foods are changed. Sets the KnownFoodLocation to true if there is any foods in the
    ///   provided list.
    /// </summary>
    /// <param name="foods">The list of known foods.</param>
    public void OnKnownFoodLocationsChanged(IReadOnlyCollection<FoodManager.FoodMemory> foods)
    {
      KnowsFoodLocation = foods.Any();
    }

    /// <summary>
    ///   Eats the provided food.
    ///   Can only take bites proportionally to it's size and cannot eat more than there is room.
    /// </summary>
    /// <param name="food">The food to eat.</param>
    public void Eat(IEatable food)
    {
      //full bite or what is left for a full stomach
      var biteSize = Math.Min(20 * SizeModifier * SizeModifier,
        _nourishmentDelegate.SaturationFromFull());
      SwallowEat(food.Consume(biteSize * Time.deltaTime));
      mouthParticles.Emit(1);
    }

    /// <summary>
    ///   Moves the Animal.
    /// </summary>
    /// <param name="destination">The position to go to.</param>
    public void GoTo(Vector3 destination)
    {
      movement.GoTo(destination);
    }

    public void StopMoving()
    {
      movement.Stop();
    }

    public void Drink(Water water)
    {
      var sip = 30 * SizeModifier * SizeModifier;
      Drink(water.SaturationModifier * sip * Time.deltaTime);
      mouthParticles.Emit(1);
    }

    /// <summary>
    ///   This method will only be called in a female animal.
    ///   spawns a random ammount of children depending on the 'maxNumberOfChildren'
    /// </summary>
    /// <param name="father"></param>
    public void SpawnMultipleChildren(AbstractAnimal father)
    {
      var amount = Random.Range(1, maxNumberOfChildren);
      while (amount >= 1)
      {
        SpawnChild(father);
        amount--;
      }
    }

    /// <summary>
    ///   This method will only be called in a female animal.
    ///   make this return the child if it needs to be overridden in the future
    /// </summary>
    public void SpawnChild(AbstractAnimal father)
    {
      Children++;
      var child = AnimalPool.SharedInstance.Get(Species);

      var speedMin = Math.Min(father.SpeedModifier, SpeedModifier);
      var speedMax = Math.Max(father.SpeedModifier, SpeedModifier);

      var sizeMin = Math.Min(father.SizeModifier, SizeModifier);
      var sizeMax = Math.Max(father.SizeModifier, SizeModifier);

      child.movement.GetAgent().Warp(transform.position);
      child.ResetGameObject(); //resets to default/random values
      child._fullyGrownSpeed = Random.Range(speedMin, speedMax);
      child._fullyGrownSize = Random.Range(sizeMin, sizeMax);
      child.InitProperties(child._fullyGrownSpeed * childrenSizeWhenBorn, child._fullyGrownSize * childrenSizeWhenBorn);
      ChildSpawnedListeners?.Invoke(child, this);

      _hoursUntilFertile = fertilityTimeInHours;
      Fertile = false;
      ShouldBirth = false;
    }

    /// <summary>
    ///   Decreases health if animal is starving or dehydrated
    /// </summary>
    private void DecreaseHealthIfStarving()
    {
      if (GetSaturation() <= 1)
        _healthDelegate.DecreaseHealth(1);

      if (GetHydration() <= 1)
        _healthDelegate.DecreaseHealth(1);
    }

    private void IncreaseHealthIfSatiated()
    {
      if (GetSaturation() >= _nourishmentDelegate.MaxSaturation * 0.75 &&
          GetSaturation() >= _nourishmentDelegate.MaxHydration * 0.75)
        _healthDelegate.IncreaseHealth(1);
    }

    protected abstract void IncreaseStaminaIfNotRunning();

    protected abstract void DecreaseStaminaIfRunning();

    public void SetMouthColor(Color color)
    {
      var main = mouthParticles.main;
      main.startColor = new ParticleSystem.MinMaxGradient(color);
      mouthParticles.Emit(1);
    }

    public AbstractAnimal GetMateTarget()
    {
      return _mateTarget;
    }

    /// <summary>
    ///   Method will only be called for females. Father parameter is for future genetic transfer implementations
    /// </summary>
    public void Mate(AbstractAnimal father)
    {
      if (Gender == Gender.Female && Fertile && !IsPregnant)
      {
        LastMaleMate = father;
        IsPregnant = true;
        Fertile = false;
        _hoursUntilPregnancy = pregnancyTimeInHours;
        PregnancyChangedListeners?.Invoke(IsPregnant);
      }
    }

    public void Forget(FoodManager.FoodMemory memory)
    {
      foodManager.Forget(memory);
    }

    public AnimalState GetCurrentStateEnum()
    {
      return _stateMachine.GetCurrentStateEnum();
    }

    public HealthDelegate GetHealthDelegate()
    {
      return _healthDelegate;
    }

    public NourishmentDelegate GetNourishmentDelegate()
    {
      return _nourishmentDelegate;
    }

    public StaminaDelegate GetStaminaDelegate()
    {
      return _staminaDelegate;
    }


    private void OnStateChanged(AnimalState state)
    {
      animationManager.ReceiveState(state, this);
    }

    /// <summary>
    ///   slightly decreases the nutritional value by 1 each second
    ///   removed if nutritional value is nothing
    /// </summary>
    public void Decay()
    {
      NutritionalValue -= Time.deltaTime * 5;
    }

    public virtual bool SafeDistanceFromEnemy()
    {
      return true;
    }

    /// <summary>
    ///   Turns the animal either away from an animal (Flee())or towards an animal (in carnivore class)
    /// </summary>
    /// <param name="animal">The animal to turn to/away from.</param>
    public void Turn(AbstractAnimal animal)
    {
      var turnSpeed = 3;
      var vectorToEnemy = transform.position - animal.transform.position;
      var rotation = Quaternion.LookRotation(vectorToEnemy);
      transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
    }

    public void Flee()
    {
      if (EnemyToFleeFrom)
      {
        Turn(EnemyToFleeFrom);
        GoTo(transform.position + transform.forward);
      }
    }

    public void SetSpeed()
    {
      switch (IsRunning)
      {
        case true when _staminaDelegate.StaminaZero:
          movement.SpeedFactor = SpeedModifier;
          break;
        case true:
          movement.SpeedFactor = RunningSpeedFactor * SpeedModifier;
          break;
        default:
          movement.SpeedFactor = SpeedModifier;
          break;
      }
    }

    private void StaminaZero(float stamina, float maxStamina)
    {
      SetSpeed();
      animationManager.SetAnimationStaminaZero(this);
    }

    public void StopFleeing()
    {
      ClearEnemyTarget();
    }


    /// <summary>
    ///   Initializes the speed, size, nutrional value ... etc.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="value2"></param>
    private void InitProperties(float speed, float size)
    {
      SpeedModifier = speed;
      SizeModifier = size;
      movement.SpeedFactor = SpeedModifier;

      InitNourishmentDelegate();

      // Setup size modification
      UpdateScale();
    }

    private void InitNourishmentDelegate()
    {
      var sizeCubed = SizeModifier * SizeModifier * SizeModifier;
      var decreaseFactor = sizeCubed + SpeedModifier * SpeedModifier;

      _nourishmentDelegate.SaturationDecreasePerHour = decreaseFactor / 2;
      _nourishmentDelegate.HydrationDecreasePerHour = decreaseFactor;
      _nourishmentDelegate.SetMaxNourishment(sizeCubed * _nourishmentMultiplier);
      NutritionalValue = _nourishmentMultiplier * sizeCubed;
      PregnancyChangedListeners += _nourishmentDelegate.OnPregnancyChanged;
    }

    private void UpdateNourishmentDelegate()
    {
      var sizeCubed = SizeModifier * SizeModifier * SizeModifier;
      var decreaseFactor = sizeCubed + SpeedModifier * SpeedModifier;

      _nourishmentDelegate.SaturationDecreasePerHour = decreaseFactor / 2;
      _nourishmentDelegate.HydrationDecreasePerHour = decreaseFactor;
      _nourishmentDelegate.UpdateMaxNourishment(sizeCubed * _nourishmentMultiplier);
      NutritionalValue = _nourishmentMultiplier * sizeCubed;
      PregnancyChangedListeners += _nourishmentDelegate.OnPregnancyChanged;
    }

    public bool NeedsNourishment()
    {
      return (IsThirsty || IsHungry) && (!KnowsFoodLocation || !KnowsWaterLocation);
    }

    #region ResetSetup

    private void ResetGender()
    {
      Gender = Random.Range(0f, 1f) > 0.5 ? Gender.Male : Gender.Female;
      RenderAnimalSpecificColors();
      if (Gender == Gender.Male) matingManager.MateListeners += OnMateFound;
    }

    private void ResetProperties()
    {
      if (IsChild) return; //child no need
      IsChild = true;
      const float rangeMin = 0.8f;
      const float rangeMax = 1.2f;
      var speed = Random.Range(rangeMin, rangeMax);
      var size = Random.Range(rangeMin, rangeMax);
      InitProperties(speed, size);
    }

    private void ResetStateMachine()
    {
      InitStateMachine(); // It also works creating a whole new state machine
    }

    public void ResetFertility()
    {
      AgeInDays = 0;
      Fertile = false;
      IsPregnant = false;
      ShouldBirth = false;
      _hoursUntilFertile = fertilityTimeInHours;
    }

    #endregion

    #region CreationSetup

    private void InitSensoryEvents()
    {
      hearing.AnimalHeardListeners += OnAnimalHeard;
      foodManager.KnownFoodMemoriesChangedListeners += OnKnownFoodLocationsChanged;
      waterManager.WaterUpdateListeners += OnWaterLocationChanged;
      vision.EnemySeenListeners += OnEnemySeen;
      _staminaDelegate.StaminaZeroListeners += StaminaZero;
    }

    private void InitStateMachine()
    {
      var states = GetStates(foodManager);
      _stateMachine = new StateMachine<AnimalState>(states, AnimalState.Wander);
      _stateMachine.StateChangedListeners += state => StateChangedListeners?.Invoke(state.ToString());

      _stateMachine.StateChangedListeners += OnStateChanged;
    }

    #endregion
  }
}