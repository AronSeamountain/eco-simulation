using System.Collections.Generic;
using System.Collections.ObjectModel;
using Foods;
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
  public delegate void KnownFoodLocationsChanged(IReadOnlyCollection<AbstractFood> food);

  [SerializeField] private VisualDetector visualDetector;
  private IList<AbstractFood> _knownFoodLocations;
  public KnownFoodLocationsChanged KnownFoodLocationsChangedListeners;
  public IReadOnlyList<AbstractFood> KnownFoodLocations => new ReadOnlyCollection<AbstractFood>(_knownFoodLocations);

  private void Start()
  {
    _knownFoodLocations = new List<AbstractFood>();
    visualDetector.FoodFoundListeners += OnFoodFound;
  }

  private void OnFoodFound(AbstractFood food)
  {
    if (_knownFoodLocations.Contains(food)) return;

    _knownFoodLocations.Add(food);
    KnownFoodLocationsChangedListeners?.Invoke(KnownFoodLocations);
  }

  public void OnFoodEaten(AbstractFood food)
  {
    if (food == null) return;

    _knownFoodLocations.Remove(food);
    KnownFoodLocationsChangedListeners?.Invoke(KnownFoodLocations);
  }
}