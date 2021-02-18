using System.Collections.Generic;
using System.Linq;
using Animal;
using AnimalStates;
using Core;
using Foods;
using UnityEngine;

/// <summary>
///   A very basic animal that searches for food.
/// </summary>
public abstract class AbstractAnimal : MonoBehaviour, ICanDrink, ICanEat, ITickable
{
  public delegate void ChildSpawned(AbstractAnimal child);

  private const int Size = 25;


  /// <summary>
  ///   The probability in the range [0, 1] whether the animal will give birth.
  /// </summary>
  [SerializeField] [Range(0f, 1f)] private float birthProbabilityPerUnit;

  [SerializeField] protected GoToMovement movement;
  [SerializeField] protected FoodManager foodManager;
  [SerializeField] protected WaterManager waterManager;
  [SerializeField] protected GameObject childPrefab;
  [SerializeField] protected EntityStatsDisplay entityStatsDisplay;

  private INewState<AnimalState> _currentState;
  private HealthDelegate _healthDelegate;
  private NourishmentDelegate _nourishmentDelegate;
  private NewStateMachine<AnimalState> _stateMachine;
  public ChildSpawned ChildSpawnedListeners;
  public bool ShouldBirth { get; private set; }
  public bool IsMoving => movement.HasTarget;

  /// <summary>
  ///   Whether the animal knows about a food location.
  /// </summary>
  public bool KnowsFoodLocation { get; private set; }

  public bool KnowsWaterLocation { get; private set; }

  /// <summary>
  ///   Returns a collection of the foods that the animal is aware of.
  /// </summary>
  public IEnumerable<FoodManager.FoodMemory> KnownFoods => foodManager.KnownFoodMemories;

  public Water ClosestKnownWater => waterManager.ClosestKnownWater;
  public bool IsHungry => _nourishmentDelegate.IsHungry;
  public bool IsThirsty => _nourishmentDelegate.IsThirsty;
  private int Health => _healthDelegate.Health;
  public bool IsAlive => Health > 0;

  protected virtual void Awake()
  {
    ShowStats(false);
    _nourishmentDelegate = new NourishmentDelegate();
    _healthDelegate = new HealthDelegate();
  }

  private void Start()
  {
    // Setup states
    var states = GetStates(foodManager);
    _stateMachine = new NewStateMachine<AnimalState>(states);
    _currentState = _stateMachine.GetCorrelatingState(AnimalState.Wander);
    _currentState.Enter();

    // Listen to food events
    foodManager.KnownFoodMemoriesChangedListeners += OnKnownFoodLocationsChanged;
    

    _healthDelegate.HealthChangedListeners += entityStatsDisplay.OnHealthChanged;
    _nourishmentDelegate.NourishmentChangedListeners += entityStatsDisplay.OnNourishmentChanged;

    //listen to water events
    waterManager.WaterUpdateListeners += OnWaterLocationChanged;
  }

  private void Update()
  {
    var newState = _currentState.Execute();
    if (newState != _currentState.GetStateEnum()) // Could be "cached" in the future.
    {
      _currentState.Exit();
      _currentState = _stateMachine.GetCorrelatingState(newState);
      _currentState.Enter();
    }
  }

  public int GetHydration()
  {
    return _nourishmentDelegate.Hydration;
  }

  public void Drink(int hydration)
  {
    _nourishmentDelegate.Hydration += hydration;
  }

  public int GetSaturation()
  {
    return _nourishmentDelegate.Saturation;
  }

  public void Eat(int saturation)
  {
    _nourishmentDelegate.Saturation += saturation;
  }

  public void Tick()
  {
    ShouldBirth = Random.Range(0f, 1f) <= birthProbabilityPerUnit;
    _nourishmentDelegate.Tick();
    foodManager.Tick();
    DecreaseHealthIfStarving();
  }

  public void DayTick()
  {
  }

  protected abstract List<INewState<AnimalState>> GetStates(FoodManager foodManager);

  protected int GetHealth()
  {
    return _healthDelegate.Health;
  }

  public void ShowStats(bool show)
  {
    entityStatsDisplay.ShowStats = show;
  }

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
    Eat(food.Consume(int.MaxValue));
  }

  /// <summary>
  ///   Moves the Animal
  /// </summary>
  /// <param name="pos">The position to go to</param>
  public void GoTo(Vector3 pos)
  {
    movement.Target = pos;
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

  public void Forget(FoodManager.FoodMemory memory)
  {
    foodManager.Forget(memory);
  }
}