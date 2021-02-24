using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Animal.Managers
{
  public sealed class HearingManager : MonoBehaviour
  {
    public delegate void KnownAnimalsChanged(IReadOnlyCollection<AnimalMemory> animalMemories);

    [SerializeField] private HearingDetector hearingDetector;
    private IList<AnimalMemory> _animalsHeard;
    public KnownAnimalsChanged KnownAnimalChangedListeners;
    public KnownAnimalsChanged UnknownAnimalChangedListeners;

    public IReadOnlyList<AnimalMemory> KnownAnimals => new ReadOnlyCollection<AnimalMemory>(_animalsHeard);

    private void Start()
    {
      _animalsHeard = new List<AnimalMemory>();
      hearingDetector.AnimalHeardListeners += OnAnimalHeard;
      hearingDetector.AnimalLeftHearingListeners += OnAnimalNotHeardAnymore;
    }

    private void OnAnimalHeard(AbstractAnimal animal)
    {
      var alreadyKnowsAnimal = false;
      foreach (var memory in _animalsHeard)
        if (memory.Animal == animal)
          alreadyKnowsAnimal = true;

      if (!alreadyKnowsAnimal)
        _animalsHeard.Add(new AnimalMemory(animal));

      KnownAnimalChangedListeners?.Invoke(KnownAnimals);
    }

    private void OnAnimalNotHeardAnymore(AbstractAnimal animal)
    {
      _animalsHeard.Remove(new AnimalMemory(animal));
      UnknownAnimalChangedListeners?.Invoke(KnownAnimals);
    }

    public class AnimalMemory : MonoBehaviour
    {
      public readonly AbstractAnimal Animal;

      public AnimalMemory(AbstractAnimal animal)
      {
        Animal = animal;
      }
    }
  }
}