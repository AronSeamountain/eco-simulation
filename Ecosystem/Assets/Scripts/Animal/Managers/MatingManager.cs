using Animal.Sensor;
using UnityEngine;

namespace Animal.Managers
{
  public class MatingManager : MonoBehaviour
  {
    public delegate void AnimalFound(AbstractAnimal animal);

    [SerializeField] private Vision vision;
    [SerializeField] private Hearing hearing;

    public AnimalFound MateListeners;

    private void Start()
    {
      vision.AnimalFoundListeners += OnAnimalFound;
      hearing.AnimalHeardListeners += OnAnimalFound;
    }

    private void OnAnimalFound(AbstractAnimal animal)
    {
      if (animal == null) return;

      if (animal.Gender == Gender.Male) return;

      MateListeners?.Invoke(animal);
    }
  }
}