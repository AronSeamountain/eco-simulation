using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Foods;
using UnityEngine;

/// <summary>
///   Takes care of finding and managing foods.
/// </summary>
public sealed class FoodManager : MonoBehaviour, ITickable
{
  /// <summary>
  ///   Gets invoked when the known food locations are changed.
  /// </summary>
  /// <param name="foodMemories">A list of all the known food sources.</param>
  public delegate void KnownFoodMemoriesChanged(IReadOnlyCollection<FoodMemory> foodMemories);

  public delegate void PreyFound(HerbivoreScript herbivore);

  [SerializeField] private VisualDetector visualDetector;
  private IList<FoodMemory> _knownFoodMemories;
  public KnownFoodMemoriesChanged KnownFoodMemoriesChangedListeners;

  public PreyFound PreyFoundListeners;

  //private CarnivoreScript carnivore;
  //public HerbivoreScript target;
  public IReadOnlyList<FoodMemory> KnownFoodMemories => new ReadOnlyCollection<FoodMemory>(_knownFoodMemories);

  private void Start()
  {
    _knownFoodMemories = new List<FoodMemory>();
    visualDetector.FoodFoundListeners += OnFoodFound;
    visualDetector.PreyFoundListeners += OnPreyFound;
  }

  public void Tick()
  {
    foreach (var memory in _knownFoodMemories) memory.Tick();

    var size = _knownFoodMemories.Count;
    _knownFoodMemories = _knownFoodMemories.Where(memory => !memory.Forgotten).ToList();
    if (size != _knownFoodMemories.Count)
      KnownFoodMemoriesChangedListeners?.Invoke(KnownFoodMemories);
  }

  public void DayTick()
  {
  }

  private void OnPreyFound(HerbivoreScript herbivore)
  {
    //if (carnivore.target == null) Debug.Log("target is null");
    //if (carnivore == null) Debug.Log("carnivore is null"); // this one triggers
    //if (herbivore == null) Debug.Log("herbivore is null");
    //carnivore.target = herbivore;
    // add herbivore to knownPreyMemory
    PreyFoundListeners?.Invoke(herbivore);
  }

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

    KnownFoodMemoriesChangedListeners?.Invoke(KnownFoodMemories);
  }

  public void Forget(FoodMemory memory)
  {
    _knownFoodMemories.Remove(memory);
    KnownFoodMemoriesChangedListeners?.Invoke(KnownFoodMemories);
  }

  public class FoodMemory : ITickable
  {
    public readonly AbstractFood Food;
    private int _timeToForget;
    public bool Forgotten;
    public Vector3 Position;

    public FoodMemory(AbstractFood food, Vector3 position)
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

    public void DayTick()
    {
    }
  }
}