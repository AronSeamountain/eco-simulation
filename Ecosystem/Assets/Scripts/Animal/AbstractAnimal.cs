using System;
using System.Collections.Generic;
using System.Linq;
using Animal.AnimalStates;
using Animal.Managers;
using Animal.Sensor;
using Animal.WorldPointFinders;
using Core;
using Foods;
using Pools;
using UI;
using UI.Properties;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Random = UnityEngine.Random;

namespace Animal
{
  /// <summary>
  ///   A very basic animal that searches for food.
  /// </summary>
  public abstract class AbstractAnimal : MonoBehaviour, ICanDrink, ICanEat, ITickable, IInspectable, IEatable,
    IResetable, IBoostable
  {
    public delegate void AgeChanged(int age);


    public delegate void AnimalDecayed(AbstractAnimal animal);

    public delegate void ChildSpawned(AbstractAnimal child, AbstractAnimal parent);

    public delegate void Died(AbstractAnimal animal);

    public delegate void PregnancyChanged(bool isPregnant);

    public delegate void PropertiesChanged();

    public delegate void StateChanged(string state);

    public virtual float RunningSpeedFactor { get; } = 5f;

    /// <summary>
    ///   The factor to decrease the speed and size with for newly spawned child animals.
    /// </summary>
    private const float ChildDecreaseValueFactor = 0.5f;

    /// <summary>
    ///   Scales the animal, is not correlated to actual size for the model logic.
    /// </summary>
    [SerializeField] private float VisualSizeModifier;

    [SerializeField] private GameObject visuals;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] protected GoToMovement movement;
    [SerializeField] protected FoodManager foodManager;
    [SerializeField] protected WaterManager waterManager;
    [SerializeField] protected GameObject childPrefab;
    [SerializeField] private MatingManager matingManager;
    [SerializeField] protected ParticleSystem mouthParticles;
    [SerializeField] protected ParticleSystem matingCue;
    [SerializeField] protected ParticleSystem pregnancyCue;
    [SerializeField] protected ParticleSystem fleeCue;
    [SerializeField] protected ParticleSystem deathCue;
    [SerializeField] protected Hearing hearing;
    [SerializeField] protected Vision vision;
    [SerializeField] private AnimationManager animationManager;
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;
    [SerializeField] private int fertilityTimeInHours = 5;
    [SerializeField] private AnimalSpecies species;
    [SerializeField] private int maxNumberOfChildren = 1;
    [SerializeField] private float pregnancyTimeInHours;
    [SerializeField] private int hoursBetweenPregnancyAndFertility;
    [SerializeField] public Collider animalCollider;
    [SerializeField] private int oldAgeThreshold = 10;
    private readonly int _nourishmentMultiplier = 100;
    private int _timeUntilRememberWater;
    private float _fleeSpeed;
    public float FullyGrownSpeed => SpeedGene.Value;
    protected HealthDelegate _healthDelegate;
    private int _hoursUntilFertile;
    private float _hoursUntilPregnancy;
    private AbstractAnimal _mateTarget;
    protected NourishmentDelegate _nourishmentDelegate;
    private float _nutritionalValue;
    protected StaminaDelegate _staminaDelegate;
    private StateMachine<AnimalState> _stateMachine;
    public float FullyGrownSize => SizeGene.Value;
    public AgeChanged AgeChangedListeners;

    public ChildSpawned ChildSpawnedListeners;
    public AnimalDecayed DecayedListeners;
    public Died DiedListeners;
    public PregnancyChanged PregnancyChangedListeners;
    public PropertiesChanged PropertiesChangedListeners;
    public Gene SpeedGene;
    public Gene SizeGene;
    public Gene VisionGene;
    public Gene HearingGene;
    public StateChanged StateChangedListeners;
    public bool IsChild { get; private set; }
    public string Uuid { get; private set; }
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
    public float SpeedModifier { get;  set; }
    public float SizeModifier { get; private set; }
    public IEatable FoodAboutTooEat { get; set; }
    public int AgeInDays { get; private set; }
    public bool ShouldBirth { get; private set; }
    public AbstractAnimal LastMaleMate { get; private set; }
    public bool Fertile { get; private set; }
    public Gender Gender { get; private set; }

    public IWorldPointFinder WorldPointFinder { get; private set; }

    public AnimalSpecies Species
    {
      get => species;
      protected set => species = value;
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
    public bool IsSatisfied => !IsHungry && !IsThirsty;
    
    [HideInInspector] public bool HasForgottenWater;

    /// <summary>
    ///   The amount of children that the animal has birthed.
    /// </summary>
    public int Children { get; set; }

    /// <summary>
    ///   The margin for which is the animal considers to have reached its desired position.
    /// </summary>
    public float Reach => SizeModifier * VisualSizeModifier * 1.3f;
    
    /// <summary>
    ///   The margin for which an animal will not change it's target point that it's running towards ( in order to reduce amount of GoTo calls)
    /// </summary>
    public float ChangeTargetThreshold => Reach * 0.8f;
    /// <summary>
    ///   Whether the animal knows about a food location.
    /// </summary>
    public bool KnowsFoodLocation { get; private set; }

    /// <summary>
    ///   Whether the animal knows about a water location.
    /// </summary>
    public bool KnowsWaterLocation { get; set; }
    
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
    }

    private void Update()
    {
      _stateMachine.Execute();
    }

    public void Boost()
    {
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
    public float GetMaxSaturation()
    {
      return _nourishmentDelegate.MaxSaturation;
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
    
    /// <summary>
    /// Gets invoked when the animals enter hunt and flee state respectively.
    /// Makes them forget location of water for some time.
    /// </summary>
    public void ForgetWaterLocationForSomeTime()
    {
      if (IsCarnivore) _timeUntilRememberWater += 66;
      else _timeUntilRememberWater += 1;
      KnowsWaterLocation = false;
      HasForgottenWater = true;
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
      Uuid = Guid.NewGuid().ToString();

      ResetWorldPointFinder();
      ResetGender();
      ResetProperties();
      ResetHealthAndActivate();
      ResetStateMachine();
      ResetFertility();

      if (EntityManager.PerformanceMode) Boost();
    }

    public void HourTick()
    {
      if (Dead) return;
      _nourishmentDelegate.HourTick();
      foodManager.HourTick();
      DecreaseHealthIfStarving();
      IncreaseHealthIfSatiated();
      DecreaseStaminaIfRunning();
      IncreaseStaminaIfNotRunning();

      if (HasForgottenWater)
      {
        _timeUntilRememberWater--;
        if (_timeUntilRememberWater <= 0)
        {
          KnowsWaterLocation = true;
          HasForgottenWater = false;
        }
      }

      if (IsPregnant)
      {
        EmitPregnancyCue();
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
      if (Dead) return;
      if (IsChild && AgeInDays >= fertilityTimeInHours / 24) IsChild = false;

      AgeInDays++;
      AgeChangedListeners?.Invoke(AgeInDays);

      if (IsChild)
      {
        var updateAmount = 1 / Mathf.Floor(fertilityTimeInHours / 24f) * ChildDecreaseValueFactor;
        SpeedModifier += FullyGrownSpeed * updateAmount;
        SizeModifier += FullyGrownSize * updateAmount;
        UpdateScale();
      }

      // if the animal is older than the old age set, it will decrease in speed
      if (AgeInDays > oldAgeThreshold)
      {
        SpeedModifier = SpeedModifier * 4 / 5;
        SetSpeed();
        PropertiesChangedListeners?.Invoke();
        //kills the animal if it is too slow, to not wait for them to actually die from being starved
        if (SpeedModifier < 0.1) _healthDelegate.DecreaseHealth(int.MaxValue);
      }
    }

    private void ResetWorldPointFinder()
    {
      WorldPointFinder = new AdjacencyListWorldPointFinderImpl();
    }

    private void ResetHealthAndActivate()
    {
      gameObject.SetActive(true);
      _healthDelegate.ResetHealth();
    }

    public virtual void UpdateScale()
    {
      var difference = visuals.transform.localScale.y;
      visuals.transform.localScale = Vector3.one * (SizeModifier * VisualSizeModifier);
      difference = visuals.transform.localScale.y - difference;
      var y = visuals.transform.localPosition.y;
      visuals.transform.localPosition =
        new Vector3(0, y + difference, 0);
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
     
      
      var dead = animal.Dead;
      if (sameTypeOfAnimal && oppositeGender && fertile && !dead &&
          (_mateTarget.DoesNotExist() || IsCloserThanPreviousMateTarget(animal)))
      {
        _mateTarget = animal;
      }
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
      HasForgottenWater = false;
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

    //Experimental method to make rabbits explore the map more
    public void ForgetFoodLocations()
    {
      foodManager.ClearFoodMemory();
      KnowsFoodLocation = false;
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
      EmitMouthParticle();
    }

    // List of visual cues to be emitted:
    public void EmitMatingCue()
    {
      if (EntityManager.PerformanceMode) return;
      matingCue.Emit(1);
    }

    public void EmitPregnancyCue()
    {
      if (EntityManager.PerformanceMode) return;
      pregnancyCue.Emit(5);
    }

    public void EmitFleeCue()
    {
      if (EntityManager.PerformanceMode) return;
      fleeCue.Emit(1);
    }

    public void EmitDeathCue()
    {
      if (EntityManager.PerformanceMode) return;
      deathCue.Emit(1);
    }

    private void EmitMouthParticle()
    {
      if (EntityManager.PerformanceMode) return;
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
      EmitMouthParticle();
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

      child.movement.GetAgent().Warp(transform.position);

      child.ResetGameObject(); //resets to default/random values

      child.SpeedGene = new Gene(father.SpeedGene, SpeedGene);
      child.SizeGene = new Gene(father.SizeGene, SizeGene);

      child.VisionGene = new Gene(father.VisionGene, VisionGene);
      child.HearingGene = new Gene(father.HearingGene, HearingGene);
      child.InitProperties(child.FullyGrownSpeed * ChildDecreaseValueFactor,
        child.FullyGrownSize * ChildDecreaseValueFactor);
      ChildSpawnedListeners?.Invoke(child, this);

      _hoursUntilFertile = hoursBetweenPregnancyAndFertility;
      Fertile = false;
      ShouldBirth = false;
      child.IsChild = true;
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
      if (GetSaturation() >= _nourishmentDelegate.MaxSaturation * 0.5 &&
          GetHydration() >= _nourishmentDelegate.MaxHydration * 0.5)
        _healthDelegate.IncreaseHealth(1);
    }

    protected abstract void IncreaseStaminaIfNotRunning();

    protected abstract void DecreaseStaminaIfRunning();

    public void SetMouthSprite(Sprite sprite)
    {
      if (EntityManager.PerformanceMode) return;
      var ts = mouthParticles.textureSheetAnimation;
      ts.SetSprite(0, sprite);
      EmitMouthParticle();
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
      if (Gender == Gender.Female && Fertile && !IsPregnant && !Dead)
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
      NutritionalValue -= Time.deltaTime * 15;
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
        var vectorToEnemy = transform.position - EnemyToFleeFrom.transform.position;
        GoTo(transform.position + vectorToEnemy.normalized * 10);
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
      _staminaDelegate.IncreaseStamina(1); // so that it can run to water source afterwards
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

      SetSensorySizes();

      InitNourishmentDelegate();

      // Setup size modification
      UpdateScale();
    }

    /// <summary>
    /// Initializes the visual area and hearing radius depending on the sesnnseDistribution Gene. 
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void SetSensorySizes()
    {
      //calculate ratio
      var totalBits = VisionGene.Bits + HearingGene.Bits;
      var hearingPercentage = HearingGene.Bits * 1f / totalBits;
      var visionPercentage = VisionGene.Bits * 1f / totalBits;

      //set visual area
      vision.SetLengthPercentage(visionPercentage);

      //set hearing area
      hearing.SetSizePercentage(hearingPercentage);
    }

    private void InitNourishmentDelegate()
    {
      var sizeCubed = SizeModifier * SizeModifier * SizeModifier;
      _nourishmentDelegate.SetMaxNourishment(sizeCubed * _nourishmentMultiplier);
      UpdateNourishmentDelegate();

      PregnancyChangedListeners += _nourishmentDelegate.OnPregnancyChanged;
    }

    private void UpdateNourishmentDelegate()
    {
      var sizeCubed = SizeModifier * SizeModifier * SizeModifier;
      var decreaseFactor = GetNourishmentDecreaseFactor();

      _nourishmentDelegate.SaturationDecreasePerHour = GetSaturationDecreaseAmountPerHour(decreaseFactor);
      _nourishmentDelegate.HydrationDecreasePerHour = GetHydrationDecreaseAmountPerHour(decreaseFactor);
      _nourishmentDelegate.UpdateMaxNourishment(sizeCubed * _nourishmentMultiplier);
      NutritionalValue = _nourishmentMultiplier * sizeCubed;
    }

    public virtual float GetHydrationDecreaseAmountPerHour(float decreaseFactor)
    {
      return decreaseFactor / 2;
    }

    public virtual float GetSaturationDecreaseAmountPerHour(float decreaseFactor)
    {
      return decreaseFactor / 4;
    }
    private float GetNourishmentDecreaseFactor()
    {
      var sizeCubed = SizeModifier * SizeModifier * SizeModifier;
      return sizeCubed + SpeedModifier * SpeedModifier;
    }

    public bool NeedsNourishment()
    {
      return (IsThirsty && !KnowsWaterLocation) || (IsHungry);
    }

    #region ResetSetup

    private void ResetGender()
    {
      Gender = Random.Range(0f, 1f) > 0.5 ? Gender.Male : Gender.Female;
      RenderAnimalSpecificColors();
      matingManager.MateListeners += OnMateFound;
      matingManager.SetGender(Gender);
    }

    private void ResetProperties()
    {
      if (IsChild) return; //child no need
      InitGenes();
      InitProperties(SpeedGene.Value, SizeGene.Value);
    }

    private void InitGenes()
    {
      SpeedGene = new Gene();
      SizeGene = new Gene();
      HearingGene = new Gene();
      VisionGene = new Gene();
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
      waterManager.InitWaterManager(this);
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