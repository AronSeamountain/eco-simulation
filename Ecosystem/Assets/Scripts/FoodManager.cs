using System.Collections.Generic;
using System.Collections.ObjectModel;
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
  public delegate void KnownFoodLocationsChanged(IReadOnlyCollection<Food> food);

  [SerializeField] private FoodDetector foodDetector;
  private IList<Food> _knownFoodLocations;
  public KnownFoodLocationsChanged KnownFoodLocationsChangedListeners;
  public IReadOnlyList<Food> KnownFoodLocations => new ReadOnlyCollection<Food>(_knownFoodLocations);

  private void Start()
  {
    _knownFoodLocations = new List<Food>();
    foodDetector.FoodFoundListeners += OnFoodFound;
  }

  private void OnFoodFound(Food food)
  {
    if (_knownFoodLocations.Contains(food)) return;

    _knownFoodLocations.Add(food);
    KnownFoodLocationsChangedListeners?.Invoke(KnownFoodLocations);
  }

  public void OnFoodEaten(Food food)
  {
    if (food == null) return;

    _knownFoodLocations.Remove(food);
    KnownFoodLocationsChangedListeners?.Invoke(KnownFoodLocations);
  }
}