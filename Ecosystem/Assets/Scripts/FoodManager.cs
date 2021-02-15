using System.Collections.Generic;
using System.Collections.ObjectModel;
using Foods;
using System.Linq;
using UnityEngine;

/// <summary>
///   Takes care of finding and managing foods.
/// </summary>
public sealed class FoodManager : MonoBehaviour, ITickable
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

  public void Tick()
  {
    foreach (var memory in _knownFoodMemories) memory.Tick();

    var size = _knownFoodMemories.Count;
    _knownFoodMemories = _knownFoodMemories.Where(memory => !memory.Forgotten).ToList();
    if (size != _knownFoodMemories.Count)
      KnownFoodLocationsChangedListeners?.Invoke(KnownFoodLocations);
  }

  private void OnFoodFound(Food food)
  private void OnFoodFound(AbstractFood food)
  {
    var alreadyKnowsFood = false;
    foreach (var memory in _knownFoodMemories)
      if (memory.Food == food)
      {
        alreadyKnowsFood = true;
        memory.Position = food.transform.position;
      }

    if (!alreadyKnowsFood)
      _knownFoodMemories.Add(new FoodMemory(food, food.transform.position));

    KnownFoodLocationsChangedListeners?.Invoke(KnownFoodLocations);
  }

  public void OnFoodEaten(AbstractFood food)
  {
    _knownFoodMemories.Remove(memory);
    KnownFoodLocationsChangedListeners?.Invoke(KnownFoodLocations);
  }

  public class FoodMemory : ITickable
  {
    public readonly Food Food;
    private int _timeToForget;
    public bool Forgotten;
    public Vector3 Position;

    public FoodMemory(Food food, Vector3 position)
    {
      Food = food;
      Position = position;
      _timeToForget = 20;
      Forgotten = false;
    }

    public void Tick()
    {
      if (_timeToForget > 0) _timeToForget -= 1;
      else Forgotten = true;
    }
  }
}