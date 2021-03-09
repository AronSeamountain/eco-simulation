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
using Random = UnityEngine.Random;

namespace Animal
{
  /// <summary>
  ///   A very basic animal that searches for food.
  /// </summary>
  public abstract class AbstractAnimal : MonoBehaviour, ICanDrink, ICanEat, ITickable, IInspectable, IEatable
  {
    public delegate void AgeChanged(int age);

    public delegate void ChildSpawned(AbstractAnimal child, AbstractAnimal parent);

    public delegate void Died(AbstractAnimal animal);

    public delegate void PropertiesChanged();

    public delegate void StateChanged(string state);


    private const float BiggestMutationChange = 0.3f;
    private const float MutationPercentPerDay = 10f;
    private const float propertyMinRange = 0.8f;
    private const float propertyMaxRange = 1.2f;

    /// <summary>
    ///   Scales the animal, is not correlated to actual size for the model logic.
    /// </summary>
    [SerializeField] private float VisualSizeModifier;

    private int FertilityTimeInDays = 5;
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
    private float _fleeSpeed;
    protected HealthDelegate _healthDelegate;
    private AbstractAnimal _mateTarget;
    protected NourishmentDelegate _nourishmentDelegate;
    private float _nutritionalValue;
    private StateMachine<AnimalState> _stateMachine;
    private int _daysUntilFertile;
    public AgeChanged AgeChangedListeners;
    public ChildSpawned ChildSpawnedListeners;
    public Died DiedListeners;
    public PropertiesChanged PropertiesChangedListeners;
    public StateChanged StateChangedListeners;
    public AbstractAnimal EnemyToFleeFrom { get; set; }
    public float SizeModifier { get; private set; }
    public float SpeedModifier { get; private set; }
    public IEatable FoodAboutTooEat { get; set; }
    public int AgeInDays { get; private set; }
    public bool ShouldBirth { get; private set; }
    public AbstractAnimal LastMaleMate { get; private set; }
    public bool Fertile { get; private set; }
    public Gender Gender { get; private set; }
    public AnimalSpecies Species { get; protected set; }
    public Water ClosestKnownWater => waterManager.ClosestKnownWater;
    public bool IsHungry => _nourishmentDelegate.IsHungry;
    public bool IsThirsty => _nourishmentDelegate.IsThirsty;
    private float Health => _healthDelegate.Health;
    public bool Alive => Health > 0;
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
    }

    private void Start()
    {
      InitStateMachine();
      InitSensoryEvents();
      InitAnimal(-1,-1);
    }

    public void FertilityTimeSetup(int time)
    {
      _daysUntilFertile = time;
      FertilityTimeInDays = time;
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

      if (_nutritionalValue >= amount)
      {
        // Eat partially
        _nutritionalValue -= amount;
        consumedFood = amount;
      }
      else
      {
        // Eat whole food
        consumedFood = _nutritionalValue;
        _nutritionalValue = 0;
      }

      if (_nutritionalValue < 0.1) FullyConsumed();

      return consumedFood;
    }

    public bool CanBeEaten()
    {
      return _nutritionalValue > 0.1;
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

    public void HourTick()
    {
      _nourishmentDelegate.HourTick();
      foodManager.HourTick();
      DecreaseHealthIfStarving();
    }

    public void DayTick()
    {
      if (!Fertile) _daysUntilFertile--;
      if (_daysUntilFertile <= 0) Fertile = true;
      AgeInDays++;
      AgeChangedListeners?.Invoke(AgeInDays);
      Mutate();
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
      transform.localScale = Vector3.one * SizeModifier;
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

      if (sameTypeOfAnimal && oppositeGender && fertile)
        _mateTarget = animal;
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
      child.InitAnimal(Random.Range(speedMin, speedMax),Random.Range(sizeMin, sizeMax));

      child.transform.position = transform.position;
      ChildSpawnedListeners?.Invoke(child, this);

      _daysUntilFertile = FertilityTimeInDays;
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
    ///   Method ill only be called for females. Father parameter is for future genetic transfer implementations
    /// </summary>
    public void Mate(AbstractAnimal father)
    {
      if (Gender == Gender.Female)
      {
        LastMaleMate = father;
        ShouldBirth = true;
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


    private void OnStateChanged(AnimalState state)
    {
      animationManager.ReceiveState(state);
    }

    /// <summary>
    ///   Removes the animal
    /// </summary>
    private void FullyConsumed()
    {
      transform.position = new Vector3(0, 10, 0); //TODO put back in ObjectPool
    }

    /// <summary>
    ///   slightly decreases the nutritional value by 1 each second
    ///   removed if nutritional value is nothing
    /// </summary>
    public void Decay()
    {
      _nutritionalValue -= Time.deltaTime;
      if (_nutritionalValue < 0.1) FullyConsumed();
    }
    /// <summary>
    ///   Sets the properties for the base animal. Override in subclass to implement species specific behaviour
    /// </summary>
    /// <param name="speed">The speed value for the animal, set to -1 if not yet specified</param>
    /// /// <param name="size">The size value for the animal, set to -1 if not yet specified</param>
    protected virtual void InitAnimal(float speed,float size)
    {
      ResetGender();
      InitSpeed(speed);
      InitSize(size);
      initNourishmentDelegateValues();
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

    public void IncreaseSpeed()
    {
      movement.SpeedFactor += 5;
    }

    public void StopFleeing()
    {
      movement.SpeedFactor = movement.SpeedFactor - 5;
      ClearEnemyTarget();
    }

    #region ResetSetup

    private void ResetGender()
    {
      Gender = Random.Range(0f, 1f) > 0.5 ? Gender.Male : Gender.Female;
      RenderAnimalSpecificColors();
      if (Gender == Gender.Male) matingManager.MateListeners += OnMateFound;
    }


    /// <summary>
    ///   Inits the values in the nourishmentDelegate
    /// </summary>
    private void initNourishmentDelegateValues()
    {
      var sizeCubed = SizeModifier * SizeModifier * SizeModifier;
      var decreaseFactor = sizeCubed + SizeModifier * SizeModifier;

      _nourishmentDelegate.SaturationDecreasePerHour = decreaseFactor / 2;
      _nourishmentDelegate.HydrationDecreasePerHour = decreaseFactor;
      _nourishmentDelegate.SetMaxNourishment(sizeCubed * 100);

      _nutritionalValue = 100 * sizeCubed;
    }

    /// <summary>
    ///   Sets the speed and size of a spawned animal
    /// </summary>
    /// <param name="speed">If set to -1 it will get a random value</param>
    private void InitSpeed(float speed)
    {
      if (speed == -1)
        SpeedModifier = Random.Range(propertyMinRange, propertyMaxRange);
      else
        SpeedModifier = speed;

      movement.SpeedFactor = SpeedModifier;
    }


    /// <summary>
    ///   Sets the size and size of a spawned animal
    /// </summary>
    /// <param name="size">If set to -1 it will get a random value</param>
    private void InitSize(float size)
    {
      if (size == -1)
        SizeModifier = Random.Range(propertyMinRange, propertyMaxRange);
      else
        SizeModifier = size;

      var scale = SizeModifier + VisualSizeModifier;
      transform.localScale = new Vector3(scale, scale, scale);
    }

    #endregion

    #region CreationSetup

    private void InitSensoryEvents()
    {
      hearing.AnimalHeardListeners += OnAnimalHeard;
      foodManager.KnownFoodMemoriesChangedListeners += OnKnownFoodLocationsChanged;
      waterManager.WaterUpdateListeners += OnWaterLocationChanged;
      vision.EnemySeenListeners += OnEnemySeen;
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