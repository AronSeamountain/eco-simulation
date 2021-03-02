using UnityEngine;

namespace Animal.Managers
{
  public sealed class HearingManager : MonoBehaviour
  {
    public delegate void KnownAnimalsChanged(AbstractAnimal animal);

    [SerializeField] private HearingDetector hearingDetector;
    //private AbstractAnimal _knownAnimal;
    public KnownAnimalsChanged KnownAnimalChangedListeners;

    private void Start()
    {
      hearingDetector.AnimalHeardListeners += OnAnimalHeard;
    }

    private void OnAnimalHeard(AbstractAnimal animal)
    {
      KnownAnimalChangedListeners?.Invoke(animal);
    }
  }
}