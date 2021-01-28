using System.Collections.Generic;
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
  private FoodEaten FoodEatenListeners;
  public IState PursueFoodState;
  public IState WanderState;

  public bool IsMoving => movement.HasTarget;

  private void Start()
  {
    foodManager.KnownFoodLocationsChangedListeners += OnKnownFoodLocationsChanged;
    FoodEatenListeners += foodManager.OnFoodEaten;

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

  private void OnKnownFoodLocationsChanged(IList<Food> foods)
  {
    // TODO: Remove me?
  }

  /// <summary>
  ///   Eats the provided food. Removes it.
  /// </summary>
  /// <param name="food">The food to eat.</param>
  public void Eat(Food food)
  {
    movement.Stop();
    FoodEatenListeners?.Invoke(food);
    Destroy(food.gameObject);
  }

  /// <summary>
  ///   Moves the Animal
  /// </summary>
  /// <param name="pos">The position to go to</param>
  public void GoTo(Vector3 pos)
  {
    movement.Target = pos;
  }

  public IList<Food> GetKnownFoods()
  {
    return foodManager.KnownFoodLocations;
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