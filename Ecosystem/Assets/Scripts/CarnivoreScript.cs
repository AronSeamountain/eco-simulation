using System.Collections.Generic;
using AnimalStates;
using Core;
using UnityEngine;

public class CarnivoreScript : Animal
{
  //public ChildSpawned ChildSpawnedListeners;
  //public bool ShouldBirth { get; private set; }
  //public bool IsMoving => movement.HasTarget;

  /// <summary>
  ///   Whether the animal knows about a food location.
  /// </summary>
  //public bool KnowsFoodLocation { get; private set; }

  //public bool KnowsWaterLocation { get; private set; } 

  /// <summary>
  ///   Returns a collection of the foods that the animal is aware of.
  /// </summary>

  public new IEnumerable<FoodManager.FoodMemory> KnownFoods => foodManager.KnownFoodMemories;

  public new Water ClosestKnownWater => waterManager.ClosestKnownWater;
  public new bool IsHungry => _nourishmentDelegate.IsHungry;
  public new bool IsThirsty => _nourishmentDelegate.IsThirsty;
  private int Health => _healthDelegate.Health;
  public new bool IsAlive => Health > 0;

  protected override void Awake()
  {
    ShowStats(false);
    _nourishmentDelegate = new NourishmentDelegate();
    _healthDelegate = new HealthDelegate();
    _healthDelegate.Health = 100;
  }

  private void Start()
  {
    movement = gameObject.GetComponent<GoToMovement>();
    foodManager = gameObject.GetComponent<FoodManager>();
    waterManager = gameObject.GetComponent<WaterManager>();

    // Setup states
    var pursueFoodState = new PursueFoodState();
    var states = new List<IState<Animal, AnimalState>>
    {
      new DeadState(),
      new WanderState(),
      pursueFoodState,
      new PursueWaterState(),
      new BirthState()
    };
    _stateMachine = new StateMachine<Animal, AnimalState>(states);
    _currentState = _stateMachine.GetCorrelatingState(AnimalState.Wander);
    _currentState.Enter(this);

    // Listen to food events
    //foodManager.KnownFoodMemoriesChangedListeners += OnKnownFoodLocationsChanged;
    //foodManager.KnownFoodMemoriesChangedListeners += pursueFoodState.OnKnownFoodLocationsChanged;

    //_healthDelegate.HealthChangedListeners += entityStatsDisplay.OnHealthChanged;
    //_nourishmentDelegate.NourishmentChangedListeners += entityStatsDisplay.OnNourishmentChanged;
    //listen to water events
    //waterManager.WaterUpdateListeners += OnWaterLocationChanged;
  }

  private void Update()
  {
    var newState = _currentState.Execute(this);
    if (newState != _currentState.GetStateEnum()) // Could be "cached" in the future.
    {
      _currentState.Exit(this);
      _currentState = _stateMachine.GetCorrelatingState(newState);
      _currentState.Enter(this);
    }

    //For debugging
    Debug.Log(gameObject + " health is: " + GetHealth());
    Debug.Log(gameObject + " is in state: " + newState);
  }
}