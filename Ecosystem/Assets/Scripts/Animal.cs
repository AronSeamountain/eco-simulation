using System;
using System.Collections.Generic;
using System.Linq;
using AnimalStates;
using UnityEngine;

/// <summary>
///   A very basic animal that searches for food.
/// </summary>
public sealed class Animal : MonoBehaviour, ICanDrink, ICanEat
{
  /// <summary>
  ///   The value for which the animal is considered hungry.
  /// </summary>
  private const int HungryNourishmentLevel = 50;

  /// <summary>
  ///   The value for which the animal is considered thirsty.
  /// </summary>
  private const int ThirstyHydrationLevel = 50;

  [SerializeField] private GoToMovement movement;
  [SerializeField] private FoodManager foodManager;
  private IState _currentState;
  private FoodEaten _foodEatenListeners;
  private int _hydration;
  private int _nourishment;
  private IList<IState> _states;

  public bool IsMoving => movement.HasTarget;

  /// <summary>
  ///   Whether the animal knows about a food location.
  /// </summary>
  public bool KnowsFoodLocation { get; private set; }

  /// <summary>
  ///   Returns a collection of the foods that the animal is aware of.
  /// </summary>
  public IReadOnlyCollection<Food> KnownFoods => foodManager.KnownFoodLocations;

  public bool IsHungry => GetNourishment() <= HungryNourishmentLevel;
  public bool IsThirsty => GetHydration() <= ThirstyHydrationLevel;

  private void Start()
  {
    // Setup states
    var pursueFoodState = new PursueFoodState();
    _states = new List<IState>
    {
      pursueFoodState,
      new WanderState()
    };
    _currentState = GetCorrelatingState(AnimalState.Wander);
    _currentState.Enter(this);

    // Listen to food events
    foodManager.KnownFoodLocationsChangedListeners += OnKnownFoodLocationsChanged;
    foodManager.KnownFoodLocationsChangedListeners += pursueFoodState.OnKnownFoodLocationsChanged;
    _foodEatenListeners += foodManager.OnFoodEaten;

    // Food and hydration
    _nourishment = 100; // Temporary
    _hydration = 100;
  }

  private void Update()
  {
    var newState = _currentState.Execute(this);
    if (newState != _currentState.GetStateEnum()) // Could be "cached" in the future.
    {
      _currentState.Exit(this);
      _currentState = GetCorrelatingState(newState);
      _currentState.Enter(this);
    }
  }

  public int GetHydration()
  {
    return _hydration;
  }

  public void Drink(int hydration)
  {
    _hydration += Mathf.Clamp(hydration, 0, int.MaxValue);
  }

  public int GetNourishment()
  {
    return _nourishment;
  }

  public void Eat(int hydration)
  {
    _nourishment += Mathf.Clamp(hydration, 0, int.MaxValue);
  }

  /// <summary>
  ///   Gets called when the list of known foods are changed. Sets the KnownFoodLocation to true if there is any foods in the
  ///   provided list.
  /// </summary>
  /// <param name="foods">The list of known foods.</param>
  private void OnKnownFoodLocationsChanged(IReadOnlyCollection<Food> foods)
  {
    KnowsFoodLocation = foods.Any();
  }

  /// <summary>
  ///   Eats the provided food. Removes it.
  /// </summary>
  /// <param name="food">The food to eat.</param>
  public void Eat(Food food)
  {
    movement.Stop();
    _foodEatenListeners?.Invoke(food);
    food.Consume();
  }

  /// <summary>
  ///   Moves the Animal
  /// </summary>
  /// <param name="pos">The position to go to</param>
  public void GoTo(Vector3 pos)
  {
    movement.Target = pos;
  }

  /// <summary>
  ///   Gets the state with the provided state enum.
  /// </summary>
  /// <param name="stateEnum">The state to get the state instance from.</param>
  /// <returns>The state correlating to the state enum.</returns>
  /// <exception cref="ArgumentOutOfRangeException">If the animal has no state for the provided state enum.</exception>
  private IState GetCorrelatingState(AnimalState stateEnum)
  {
    var state = _states.First(s => s.GetStateEnum() == stateEnum);
    if (state != null) return state;

    throw new ArgumentOutOfRangeException(nameof(state), stateEnum, null);
  }

  public void StopMoving()
  {
    movement.Stop();
  }

  /// <summary>
  ///   Gets invoked when the animal eats a food.
  /// </summary>
  /// <param name="food">The food that was eaten.</param>
  private delegate void FoodEaten(Food food);
}