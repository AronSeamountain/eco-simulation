using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Serialization;

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

  public delegate void KnownClosestWaterChanged(Water water);

  [SerializeField] private FoodDetector foodDetector;
  private IList<Food> _knownFoodLocations;
  public KnownFoodLocationsChanged KnownFoodLocationsChangedListeners;
  public IReadOnlyList<Food> KnownFoodLocations => new ReadOnlyCollection<Food>(_knownFoodLocations);

  public KnownClosestWaterChanged WaterUpdateListeners; 
  public Water ClosestKnownWater;

  private void Start()
  {
    _knownFoodLocations = new List<Food>();
    foodDetector.FoodFoundListeners += OnFoodFound;
    foodDetector.WaterFoundListeners += OnWaterFound;
  }

  private void OnFoodFound(Food food)
  {
    if (_knownFoodLocations.Contains(food)) return;

    _knownFoodLocations.Add(food);
    KnownFoodLocationsChangedListeners?.Invoke(KnownFoodLocations);
  }

  private void OnWaterFound(Water water)
  {
    if (water == null) return;
    ClosestKnownWater = water;
    Debug.Log("Closest water source found: " + water);
    
    WaterUpdateListeners?.Invoke(ClosestKnownWater);
  }
  
  public void OnFoodEaten(Food food)
  {
    if (food == null) return;

    _knownFoodLocations.Remove(food);
    KnownFoodLocationsChangedListeners?.Invoke(KnownFoodLocations);
  }
}