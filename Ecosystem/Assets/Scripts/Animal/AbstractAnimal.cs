using System;
using System.Collections.Generic;
using System.Linq;
using Animal.AnimalStates;
using Animal.Managers;
using Core;
using Foods;
using UI;
using UI.Properties;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
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

    public delegate void StateChanged(string state);

    public enum AnimalType
    {
      Carnivore,
      Herbivore
    }

    private int FertilityTimeInDays = 5;
    [SerializeField] protected GoToMovement movement;
    [SerializeField] protected FoodManager foodManager;
    [SerializeField] protected WaterManager waterManager;
    [SerializeField] protected GameObject childPrefab;
    [SerializeField] private MatingManager matingManager;
    [SerializeField] protected ParticleSystem mouthParticles;
    [SerializeField] protected HearingManager hearingManager;
    [SerializeField] private AnimationManager animationManager;
    [SerializeField] private SkinnedMeshRenderer genderRenderer;
    public AbstractAnimal enemyToFleeFrom;
    private float _fleeSpeed;
    protected HealthDelegate _healthDelegate;
    private AbstractAnimal _mateTarget;
    protected NourishmentDelegate _nourishmentDelegate;
    private float _sizeModifier;
    private float _speedModifier;
    private float _nutritionalValue;
    private StateMachine<AnimalState> _stateMachine;
    private int _daysUntilFertile;
    public AgeChanged AgeChangedListeners;
    public ChildSpawned ChildSpawnedListeners;
    public StateChanged StateChangedListeners;
    public IEatable FoodAboutTooEat { get; set; }
    public int AgeInDays { get; private set; }
    public bool ShouldBirth { get; private set; }
    public bool Fertile { get; private set; }
    public bool IsMoving => movement.IsMoving;
    public Gender Gender { get; private set; }
    public AnimalType Type { get; protected set; }

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

    public Water ClosestKnownWater => waterManager.ClosestKnownWater;
    public bool IsHungry => _nourishmentDelegate.IsHungry;
    public bool IsThirsty => _nourishmentDelegate.IsThirsty;
    private float Health => _healthDelegate.Health;
    public bool IsAlive => Health > 0;
    public bool IsCarnivore => Type == AnimalType.Carnivore;
    public bool IsHerbivore => Type == AnimalType.Herbivore;

    private void Awake()
    {
      _nourishmentDelegate = new NourishmentDelegate();
      _healthDelegate = new HealthDelegate();
    }

    private void Start()
    {
      var states = GetStates(foodManager);
      _stateMachine = new StateMachine<AnimalState>(states, AnimalState.Wander);
      _stateMachine.StateChangedListeners += state => StateChangedListeners?.Invoke(state.ToString());

      _stateMachine.StateChangedListeners += SendState;
      
      // Setup gender
      GenerateGender();
      if (Gender == Gender.Male) matingManager.MateListeners += OnMateFound;

      FertilitySetup(FertilityTimeInDays);
      //Listen to hearing events
      hearingManager.KnownAnimalChangedListeners += OnAnimalHeard;

      // Listen to food events
      foodManager.KnownFoodMemoriesChangedListeners += OnKnownFoodLocationsChanged;

      // Listen to water events
      waterManager.WaterUpdateListeners += OnWaterLocationChanged;

      // Setup speed and size variables for nourishment modifiers
      const float rangeMin = 0.8f;
      const float rangeMax = 1.2f;
      _speedModifier = Random.Range(rangeMin, rangeMax); //TODO make modified based on parent
      _sizeModifier = Random.Range(rangeMin, rangeMax); //TODO make modified based on parent
      var sizeCubed = _sizeModifier * _sizeModifier * _sizeModifier;
      var decreaseFactor = (float) (sizeCubed + Math.Pow(_speedModifier, 2));

      _nourishmentDelegate.SaturationDecreasePerHour = decreaseFactor / 2;
      _nourishmentDelegate.HydrationDecreasePerHour = decreaseFactor;
      _nourishmentDelegate.SetMaxNourishment((float) Math.Pow(_sizeModifier, 3) * 100);

      // Setup speed modifier

      movement.SpeedFactor = _speedModifier;

      // Setup size modification
      transform.localScale = new Vector3(_sizeModifier, _sizeModifier, _sizeModifier);
      _nutritionalValue = 100 * sizeCubed;

      AnimalSetup();
    }

    public void FertilitySetup(int time)
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

    public IList<AbstractProperty> GetStats(bool isTargeted)
    {
      var hearingDetector = GetComponentInChildren<HearingDetector>();
      var visualDetector = GetComponentInChildren<VisualDetector>();
      hearingDetector.GetComponent<Renderer>().enabled = isTargeted;
      visualDetector.GetComponent<Renderer>().enabled = isTargeted;

      if (!isTargeted) return null;

      return PropertiesFactory.Create(this);
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
    }

    public bool CanEatMore()
    {
      return _nourishmentDelegate.SaturationIsFull();
    }

    public bool CanDrinkMore()
    {
      return _nourishmentDelegate.HydrationIsFull();
    }

    protected abstract void AnimalSetup();

    protected virtual void OnAnimalHeard(AbstractAnimal animal)
    {
      // do different things in herbivore and carnivore.
    }

    private void ClearEnemyTarget()
    {
      enemyToFleeFrom = null;
    }

    private void GenerateGender()
    {
      var random = Random.Range(0f, 1f);
      Fertile = false;
      if (random > 0.5)
      {
        Gender = Gender.Male;
        genderRenderer.material.SetColor("_Color", Color.cyan);
      }
      else
      {
        Gender = Gender.Female;
        genderRenderer.material.SetColor("_Color", Color.magenta);
      }
    }

    private void OnMateFound(AbstractAnimal animal)
    {
      var sameTypeOfAnimal = animal.Type == Type;
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
      var biteSize = Math.Min(20 * _sizeModifier * _sizeModifier,
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
      var sip = 30 * _sizeModifier * _sizeModifier;
      Drink(water.SaturationModifier * sip * Time.deltaTime);
      mouthParticles.Emit(1);
    }

    public void SpawnChild()
    {
      Children++;
      var child = Instantiate(childPrefab, transform.position, Quaternion.identity).GetComponent<AbstractAnimal>();
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
      if (Gender == Gender.Female) ShouldBirth = true;
    }

    public void Forget(FoodManager.FoodMemory memory)
    {
      foodManager.Forget(memory);
    }

    public AnimalState GetCurrentStateEnum()
    {
      return _stateMachine.GetCurrentStateEnum();
    }

    public float GetSize()
    {
      return transform.localScale.x;
    }

    public HealthDelegate GetHealthDelegate()
    {
      return _healthDelegate;
    }

    public NourishmentDelegate GetNourishmentDelegate()
    {
      return _nourishmentDelegate;
    }

    public float GetSpeedModifier()
    {
      return _speedModifier;
    }

    private void SendState(AnimalState state)
    {
      animationManager.ReceiveState(state);
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

      if (_nutritionalValue < 0.1)
      {
        FullyConsumed();
      }

      return consumedFood;
    }

    /// <summary>
    /// Removes the animal
    /// </summary>
    private void FullyConsumed()
    {
      transform.position = new Vector3(0, 10, 0); //TODO put back in ObjectPool
    }

    public bool CanBeEaten()
    {
      return _nutritionalValue > 0.1;
    }

    /// <summary>
    /// slightly decreases the nutritional value by 1 each second
    /// removed if nutritional value is nothing
    /// </summary>
    public void Decay()
    {
      _nutritionalValue -= Time.deltaTime;
      if (_nutritionalValue < 0.1)
      {
        FullyConsumed();
      }
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
      if (enemyToFleeFrom)
      {
        Turn(enemyToFleeFrom);
        GoTo(transform.position + transform.forward);
      }
    }
    public void IncreaseSpeed()
    {
      movement.SpeedFactor = movement.SpeedFactor + 5;
    }

    public void StopFleeing()
    {
      movement.SpeedFactor = movement.SpeedFactor - 5;
      ClearEnemyTarget();
    }
  }
}