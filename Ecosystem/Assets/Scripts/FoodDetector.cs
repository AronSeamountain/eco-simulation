﻿using UnityEngine;

/// <summary>
///   Scans for food and calls a delegate when food is found.
/// </summary>
public sealed class FoodDetector : MonoBehaviour
{
  /// <summary>
  ///   Gets invoked when a food is found.
  /// </summary>
  /// <param name="food">The found that was just found.</param>
  public delegate void FoodFound(Food food);

  public FoodFound FoodFoundListeners;

  private void OnTriggerEnter(Collider other)
  {
    if (other.GetComponent<Food>() is Food food) FoodFoundListeners?.Invoke(food);
  }
}