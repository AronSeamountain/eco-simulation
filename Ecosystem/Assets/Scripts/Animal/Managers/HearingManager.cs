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

    public IReadOnlyList<AnimalMemory> KnownAnimals => new ReadOnlyCollection<AnimalMemory>(_animalsHeard);


    private void Start()
    {
      _animalsHeard = new List<AnimalMemory>();
      hearingDetector.AnimalHeardListeners += OnAnimalHeard;
      hearingDetector.AnimalLeftHearingListeners += OnAnimalNotHeard;
    }

    private void OnAnimalHeard(AbstractAnimal animal)
    {
      var alreadyKnowsAnimal = false;
      foreach (var memory in _animalsHeard)
        if (memory.Animal == animal)
          alreadyKnowsAnimal = true;

      if (!alreadyKnowsAnimal)
        _animalsHeard.Add(gameObject.AddComponent<AnimalMemory>());

      KnownAnimalChangedListeners?.Invoke(KnownAnimals);
    }

    private void OnAnimalNotHeard(AbstractAnimal animal)
    {
      _animalsHeard.Remove(gameObject.AddComponent<AnimalMemory>());
      KnownAnimalChangedListeners?.Invoke(KnownAnimals);
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