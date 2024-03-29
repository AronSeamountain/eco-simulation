using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Animal.Sensor;
using Core;
using Foods;
using UnityEngine;

namespace Animal.Managers
{
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

    public delegate void PreyFound(Herbivore herbivore);

    [SerializeField] private Vision vision;
    private IList<FoodMemory> _knownFoodMemories;
    public KnownFoodMemoriesChanged KnownFoodMemoriesChangedListeners;

    public PreyFound PreyFoundListeners;

    public IReadOnlyList<FoodMemory> KnownFoodMemories => new ReadOnlyCollection<FoodMemory>(_knownFoodMemories);

    private void Start()
    {
      _knownFoodMemories = new List<FoodMemory>();
      vision.FoodFoundListeners += OnFoodFound;
      vision.PreyFoundListeners += OnPreyFound;
    }

    public void HourTick()
    {
      foreach (var memory in _knownFoodMemories) memory.HourTick();

      var size = _knownFoodMemories.Count;
      _knownFoodMemories = _knownFoodMemories.Where(memory => !memory.Forgotten).ToList();
      if (size != _knownFoodMemories.Count)
        KnownFoodMemoriesChangedListeners?.Invoke(KnownFoodMemories);
    }

    public void DayTick()
    {
    }
    

    private void OnPreyFound(Herbivore herbivore)
    {
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

    public void ClearFoodMemory()
    {
      _knownFoodMemories.Clear();
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

      public void HourTick()
      {
        if (_timeToForget > 0) _timeToForget -= 1;
        else Forgotten = true;
      }

      public void DayTick()
      {
      }
    }
  }
}