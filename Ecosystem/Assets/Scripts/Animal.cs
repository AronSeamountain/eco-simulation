using System.Collections.Generic;
using System.Linq;
using AnimalStates;
using UnityEngine;

/// <summary>
///   A very basic animal that searches for food.
/// </summary>
public sealed class Animal : MonoBehaviour
{
  [SerializeField] private GoToMovement movement;
  [SerializeField] private FoodManager foodManager;
  private IState _currentState;
  private FoodEaten _foodEatenListeners;
  public IState PursueFoodState;
  public IState WanderState;

  public bool IsMoving => movement.HasTarget;

  /// <summary>
  ///   Whether the animal knows about a food location.
  /// </summary>
  public bool KnowsFoodLocation { get; private set; }

  private void Start()
  {
    foodManager.KnownFoodLocationsChangedListeners += OnKnownFoodLocationsChanged;
    _foodEatenListeners += foodManager.OnFoodEaten;

    WanderState = new WanderState();
    PursueFoodState = new PursueFoodState();
    _currentState = WanderState;

    _currentState.Enter(this);
  }

  private void Update()
  {
    var newState = _currentState.Execute(this);
    if (newState != _currentState)
    {
      _currentState.Exit(this);
      _currentState = newState;
      _currentState.Enter(this);
    }
  }

  /// <summary>
  ///   Gets called when the list of known foods are changed. Sets the KnownFoodLocation to true if there is any foods in the provided list.
  /// </summary>
  /// <param name="foods">The list of known foods.</param>
  private void OnKnownFoodLocationsChanged(IList<Food> foods)
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
  ///   Returns a list of the foods that the animal knows of.
  /// </summary>
  public IList<Food> KnownFoods => foodManager.KnownFoodLocations;

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