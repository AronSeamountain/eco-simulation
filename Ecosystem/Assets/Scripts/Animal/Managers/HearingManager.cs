using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Animal.Managers
{
  public sealed class HearingManager : MonoBehaviour
  {
    public delegate void KnownAnimalsChanged(AbstractAnimal animal);

    [SerializeField] private HearingDetector hearingDetector;
    public KnownAnimalsChanged KnownAnimalChangedListeners;
    private AbstractAnimal _knownAnimal;

    private void Start()
    {
      hearingDetector.AnimalHeardListeners += OnAnimalHeard;
    }

    private void OnAnimalHeard(AbstractAnimal animal)
    {
      KnownAnimalChangedListeners?.Invoke(_knownAnimal);
    }
    
  }
}