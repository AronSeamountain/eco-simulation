using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   Takes care of finding and managing foods.
/// </summary>
public sealed class FoodManager : MonoBehaviour
{
  /// <summary>
  ///   Gets invoked when the known food locations are changed.
  /// </summary>
  /// <param name="food">A list of all the known food sources.</param>
  public delegate void KnownFoodLocationsChanged(IList<Food> food);

  [SerializeField] private FoodDetector foodDetector;

  public KnownFoodLocationsChanged KnownFoodLocationsChangedListeners;
  public IList<Food> KnownFoodLocations { get; } = new List<Food>();

  private void Start()
  {
    foodDetector.FoodFoundListeners += OnFoodFound;
  }

  private void OnFoodFound(Food food)
  {
    if (!KnownFoodLocations.Contains(food)) // Check if already aware of the food.
    {
      KnownFoodLocations.Add(food);
      KnownFoodLocationsChangedListeners?.Invoke(KnownFoodLocations);
    }
  }

  public void OnFoodEaten(Food food)
  {
    if (food == null) return;

    KnownFoodLocations.Remove(food);
    KnownFoodLocationsChangedListeners?.Invoke(KnownFoodLocations);
  }
}