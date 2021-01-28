﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///   A very basic animal that searches for food.
/// </summary>
public sealed class Animal : MonoBehaviour
{
  [SerializeField] private GoToMovement movement;
  [SerializeField] private FoodManager foodManager;
  private Food _foodTarget;

  /// <summary>
  ///   Gets invoked when the animal eats a food.
  /// </summary>
  /// <param name="food">The food that was eaten.</param>
  private delegate void FoodEaten(Food food);

  private FoodEaten FoodEatenListeners;

  /// <summary>
  ///   Whether the animal has a food target.
  ///   TODO: Should be converted to a plain bool like in GoTo movement. Null comparision is expensive.
  /// </summary>
  private bool HasFoodTarget => _foodTarget != null;

  private void Start()
  {
    foodManager.KnownFoodLocationsChangedListeners += OnKnownFoodLocationsChanged;
    FoodEatenListeners += foodManager.OnFoodEaten;
  }

  private void OnKnownFoodLocationsChanged(IList<Food> foods)
  {
    if (foods == null || !foods.Any()) return;

    var closestFood = GetClosestFood(foods);
    movement.Target = closestFood.transform;
    _foodTarget = closestFood;
  }

  private void Update()
  {
    if (!HasFoodTarget) return;

    if (Distance(_foodTarget) < 2)
      Eat(_foodTarget);
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
    return (food.transform.position - this.transform.position).magnitude;
  }

  /// <summary>
  ///   Eats the provided food. Removes it.
  /// </summary>
  /// <param name="food">The food to eat.</param>
  private void Eat(Food food)
  {
    movement.Target = null;
    FoodEatenListeners?.Invoke(food);
    Destroy(food.gameObject);
  }
}