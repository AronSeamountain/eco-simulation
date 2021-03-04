using UnityEngine;

namespace Animal.Managers
{
  public sealed class HearingDetector : MonoBehaviour
  {
    public delegate void KnownAnimalsChanged(AbstractAnimal animal);

    [SerializeField] private Animal.HearingDetector hearingDetector;
    private AbstractAnimal _knownAnimal;
    public KnownAnimalsChanged KnownAnimalChangedListeners;

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