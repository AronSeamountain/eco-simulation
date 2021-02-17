using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public sealed class PreyManager : MonoBehaviour, ITickable
{
  /// <summary>
  ///   Gets invoked when the known food locations are changed (the prey for the carnivore).
  /// </summary>
  /// <param name="foodMemories">A list of all the known food sources.</param>
  public delegate void KnownFoodMemoriesChanged(IReadOnlyCollection<PreyMemory> foodMemories);

  [SerializeField] private VisualDetector visualDetector;
  private IList<PreyMemory> _knownPreyMemories;
  public KnownFoodMemoriesChanged KnownPreyMemoriesChangedListeners;
  public IReadOnlyList<PreyMemory> KnownPreyMemories => new ReadOnlyCollection<PreyMemory>(_knownPreyMemories);

  private void Start()
  {
    _knownPreyMemories = new List<PreyMemory>();
    visualDetector.PreyFoundListeners += OnFoodFound;
  }

  public void Tick()
  {
    foreach (var memory in _knownPreyMemories) memory.Tick();

    var size = _knownPreyMemories.Count;
    _knownPreyMemories = _knownPreyMemories.Where(memory => !memory.Forgotten).ToList();
    if (size != _knownPreyMemories.Count)
      KnownPreyMemoriesChangedListeners?.Invoke(KnownPreyMemories);
  }

  public void DayTick()
  {
  }

  private void OnFoodFound(HerbivoreScript animal)
  {
    var alreadyKnowsFood = false;
    foreach (var memory in _knownPreyMemories)
      if (memory.Animal == animal)
      {
        alreadyKnowsFood = true;
        memory.Position = animal.transform.position;
      }

    if (!alreadyKnowsFood)
      _knownPreyMemories.Add(new PreyMemory(animal, animal.transform.position));

    KnownPreyMemoriesChangedListeners?.Invoke(KnownPreyMemories);
  }

  public void Forget(PreyMemory memory)
  {
    _knownPreyMemories.Remove(memory);
    KnownPreyMemoriesChangedListeners?.Invoke(KnownPreyMemories);
  }

  public class PreyMemory : ITickable
  {
    public readonly HerbivoreScript Animal;
    private int _timeToForget;
    public bool Forgotten;
    public Vector3 Position;

    public PreyMemory(HerbivoreScript animal, Vector3 position)
    {
      Animal = animal;
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