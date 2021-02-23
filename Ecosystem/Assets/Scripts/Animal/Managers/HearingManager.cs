using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Core;
using UnityEngine;

namespace Animal.Managers
{
  public sealed class HearingManager : MonoBehaviour, ITickable
  {
    public delegate void KnownAnimalsChanged(IReadOnlyCollection<AnimalMemory> animalMemories);
    
    [SerializeField] private HearingDetector hearingDetector;
    private IList<AnimalMemory> _animalsHeard;
    public KnownAnimalsChanged KnownAnimalMemoriesChangedListeners;
    
    public IReadOnlyList<AnimalMemory> KnownAnimals => new ReadOnlyCollection<AnimalMemory>(_animalsHeard);

    
    private void Start()
    {
      _animalsHeard= new List<AnimalMemory>();
      hearingDetector.AnimalHeardListeners += OnAnimalHeard;
    }

    public void Tick()
    {
      foreach (var memory in _animalsHeard) memory.Tick();

      var size = _animalsHeard.Count;
      _animalsHeard = _animalsHeard.Where(memory => !memory.Forgotten).ToList();
      if (size != _animalsHeard.Count)
        KnownAnimalMemoriesChangedListeners?.Invoke(KnownAnimals);
    }

    public void DayTick()
    {
    }

    private void OnAnimalHeard(AbstractAnimal animal)
    {
      var alreadyKnowsAnimal = false;
      foreach (var memory in _animalsHeard)
        if (memory.Animal == animal)
        {
          alreadyKnowsAnimal= true;
        }

      if (!alreadyKnowsAnimal)
        _animalsHeard.Add(new AnimalMemory(animal));

      KnownAnimalMemoriesChangedListeners?.Invoke(KnownAnimals);
    }

    public class AnimalMemory : ITickable
    {
      public readonly AbstractAnimal Animal;
      private int _timeToForget;
      public bool Forgotten;

      public AnimalMemory(AbstractAnimal animal)
      {
        Animal = animal;
        _timeToForget = 10;
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

}