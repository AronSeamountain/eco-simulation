using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   Takes care of finding and managing foods.
/// </summary>
public sealed class FoodManager : MonoBehaviour
{
  [SerializeField] private FoodDetector foodDetector;
  private readonly IList<Food> _availableFoods = new List<Food>();

  /// <summary>
  ///   Gets invoked when the known food locations are changed.
  /// </summary>
  /// <param name="food">A list of all the known food sources.</param>
  public delegate void KnownFoodLocationsChanged(IList<Food> food);

  public KnownFoodLocationsChanged KnownFoodLocationsChangedListeners;

  private void Start()
  {
    foodDetector.FoodFoundListeners += OnFoodFound;
  }

  private void OnFoodFound(Food food)
  {
    if (!_availableFoods.Contains(food)) // Check if already aware of the food.
      {
        _availableFoods.Add(food);
        KnownFoodLocationsChangedListeners?.Invoke(_availableFoods);
      }
  }

  public void OnFoodEaten(Food food)
  {
    if (food == null) return;

    _availableFoods.Remove(food);
    KnownFoodLocationsChangedListeners?.Invoke(_availableFoods);
  }
}