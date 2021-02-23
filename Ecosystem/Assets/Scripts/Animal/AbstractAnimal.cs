using System;
using System.Collections.Generic;
using System.Linq;
using Animal.AnimalStates;
using Animal.Managers;
using Core;
using Foods;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animal
{
  /// <summary>
  ///   A very basic animal that searches for food.
  /// </summary>
  public abstract class AbstractAnimal : MonoBehaviour, ICanDrink, ICanEat, ITickable, IStatable
  {
    public delegate void ChildSpawned(AbstractAnimal child);

    private const int FertilityTimeInUnits = 5;

    /// <summary>
    ///   The probability in the range [0, 1] whether the animal will give birth.
    /// </summary>
    [SerializeField] [Range(0f, 1f)] private float birthProbabilityPerUnit;

    [SerializeField] protected GoToMovement movement;
    [SerializeField] protected FoodManager foodManager;
    [SerializeField] protected WaterManager waterManager;
    [SerializeField] protected GameObject childPrefab;
    [SerializeField] private MatingManager matingManager;
    [SerializeField] protected ParticleSystem mouthParticles;
    protected HealthDelegate _healthDelegate;
    private AbstractAnimal _mateTarget;
    protected NourishmentDelegate _nourishmentDelegate;
    private float _sizeModifier;
    private float _speedModifier;
    private StateMachine<AnimalState> _stateMachine;
    private int _unitsUntilFertile = FertilityTimeInUnits;
    public ChildSpawned ChildSpawnedListeners;
    public bool ShouldBirth { get; private set; }
    public bool Fertile { get; private set; }
    public bool IsMoving => movement.IsMoving;
    public Gender Gender { get; private set; }

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

    private void Awake()
    {
      _nourishmentDelegate = new NourishmentDelegate();
      _healthDelegate = new HealthDelegate();
    }

    private void Start()
    {
      var states = GetStates(foodManager);
      _stateMachine = new StateMachine<AnimalState>(states, AnimalState.Wander);

      // Setup gender
      GenerateGender();
      if (Gender == Gender.Male) matingManager.MateListeners += OnMateFound;

      // Listen to food events
      foodManager.KnownFoodMemoriesChangedListeners += OnKnownFoodLocationsChanged;

      // Listen to water events
      waterManager.WaterUpdateListeners += OnWaterLocationChanged;

      // Setup speed and size variables for nourishment modifiers
      const float rangeMin = 0.8f;
      const float rangeMax = 1.2f;
      _speedModifier = Random.Range(rangeMin, rangeMax); //TODO make modified based on parent
      _sizeModifier = Random.Range(rangeMin, rangeMax); //TODO make modified based on parent

      var decreaseFactor = (float) (Math.Pow(_sizeModifier, 3) + Math.Pow(_speedModifier, 2));

      _nourishmentDelegate.SaturationDecreasePerUnit = decreaseFactor / 2;
      _nourishmentDelegate.HydrationDecreasePerUnit = decreaseFactor;
      _nourishmentDelegate.SetMaxNourishment((float) Math.Pow(_sizeModifier, 3) * 100);

      // Setup speed modifier
      movement.SpeedFactor = _speedModifier;

      // Setup size modification
      transform.localScale = new Vector3(_sizeModifier, _sizeModifier, _sizeModifier);
    }

    private void Update()
    {
      _stateMachine.Execute();
    }

    public float GetHydration()
    {
      return _nourishmentDelegate.Hydration;
    }

    public void Drink(int hydration)
    {
      _nourishmentDelegate.Hydration += hydration;
    }

    public float GetSaturation()
    {
      return _nourishmentDelegate.Saturation;
    }

    public void Eat(int saturation)
    {
      _nourishmentDelegate.Saturation += saturation;
    }

    public IList<GameObject> GetStats(bool isTargeted)
    {
      var visualDetector = GetComponentInChildren<VisualDetector>();
      visualDetector.GetComponent<Renderer>().enabled = isTargeted;

      if (!isTargeted) return null;

      return PropertyFactory.MakeAnimalObjects(this);
    }


    public void Tick()
    {
      ShouldBirth = Random.Range(0f, 1f) <= birthProbabilityPerUnit;

      if (!Fertile) _unitsUntilFertile--;

      if (_unitsUntilFertile <= 0) Fertile = true;

      ShouldBirth = Random.Range(0f, 1f) <= birthProbabilityPerUnit;
      _nourishmentDelegate.Tick();
      foodManager.Tick();
      DecreaseHealthIfStarving();
    }

    public void DayTick()
    {
    }

    private void GenerateGender()
    {
      var random = Random.Range(0f, 1f);
      var cubeRenderer = gameObject.GetComponent<Renderer>();
      if (random > 0.5)
      {
        Gender = Gender.Male;
        Fertile = false;
        cubeRenderer.material.SetColor("_Color", Color.cyan);
      }
      else
      {
        Gender = Gender.Female;
        Fertile = false;
        cubeRenderer.material.SetColor("_Color", Color.magenta);
      }
    }

    private void OnMateFound(AbstractAnimal animal)
    {
      if (animal.Gender != Gender && animal.Fertile) _mateTarget = animal;
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
    /// </summary>
    /// <param name="food">The food to eat.</param>
    public void Eat(AbstractFood food)
    {
      Eat(food.Consume((int) Math.Round(20*_sizeModifier*_sizeModifier)));
      //insert code to pause and particles
      mouthParticles.Play();
      
    }

    public void StopEating()
    {
      mouthParticles.Stop();
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
      Drink(water.Hydration);
    }

    public void SpawnChild()
    {
      var child = Instantiate(childPrefab, transform.position, Quaternion.identity).GetComponent<AbstractAnimal>();
      ChildSpawnedListeners?.Invoke(child);

      _unitsUntilFertile = FertilityTimeInUnits;
      Fertile = false;
      ShouldBirth = false;
    }

    /// <summary>
    ///   Decreases health if animal is starving and dehydrated
    /// </summary>
    private void DecreaseHealthIfStarving()
    {
      if (GetSaturation() <= 10 || GetHydration() <= 10)
        _healthDelegate.DecreaseHealth(1);
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
  }
}