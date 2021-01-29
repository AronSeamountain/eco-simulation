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
  private IState _currentState = new WanderState();
  private Food _foodTarget;
  private FoodEaten _foodEatenListeners;

  /// <summary>
  ///   Whether the animal has a food target.
  ///   TODO: Should be converted to a plain bool like in GoTo movement. Null comparision is expensive.
  /// </summary>
  private bool HasFoodTarget => _foodTarget != null;

  public bool IsMoving => movement.HasTarget;

  private void Start()
  {
    foodManager.KnownFoodLocationsChangedListeners += OnKnownFoodLocationsChanged;
    _foodEatenListeners += foodManager.OnFoodEaten;
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

    return;
    if (!HasFoodTarget) return;

    if (Distance(_foodTarget) < 2)
      Eat(_foodTarget);
  }

  private void OnKnownFoodLocationsChanged(IList<Food> foods)
  {
    return;
    if (foods == null || !foods.Any()) return;

    var closestFood = GetClosestFood(foods);
    movement.Target = closestFood.transform.position;
    _foodTarget = closestFood;
  }

  /// <summary>
  ///   Returns the food in the provided list that is the closest to the game object. Returns null if foods is null.
  /// </summary>
  /// <param name="foods">The list of foods to search from.</param>
  /// <returns>The closest food.</returns>
  private Food GetClosestFood(ICollection<Food> foods)
  {
    if (foods.Count < 0) return null;

    return foods.OrderBy(Distance).First();
  }

  /// <summary>
  ///   Gets the distance from the game animal to the provided food.
  /// </summary>
  /// <param name="food">The food to check distance to.</param>
  /// <returns>The distance to the provided food.</returns>
  private float Distance(Food food)
  {
    return (food.transform.position - transform.position).magnitude;
  }

  /// <summary>
  ///   Eats the provided food. Removes it.
  /// </summary>
  /// <param name="food">The food to eat.</param>
  private void Eat(Food food)
  {
    movement.Stop();
    _foodEatenListeners?.Invoke(food);
    Destroy(food.gameObject);
  }

  /// <summary>
  ///   Moves the Animal
  /// </summary>
  /// <param name="pos">The position to go to</param>
  public void GoTo(Vector3 pos)
  {
    Debug.Log("lala Land");
    movement.Target = pos;
  }

  /// <summary>
  ///   Gets invoked when the animal eats a food.
  /// </summary>
  /// <param name="food">The food that was eaten.</param>
  private delegate void FoodEaten(Food food);
}