using Animal.Sensor;
using UnityEngine;

namespace Animal.Managers
{
  public class MatingManager : MonoBehaviour
  {
    public delegate void AnimalFound(AbstractAnimal animal);

    [SerializeField] private Vision vision;

    public AnimalFound MateListeners;

    private void Start()
    {
      vision.AnimalFoundListeners += OnAnimalFound;
    }

    private void OnAnimalFound(AbstractAnimal animal)
    {
      if (animal == null) return;

      if (animal.Gender == Gender.Male) return;

      if (MateListeners != null) MateListeners.Invoke(animal);
    }
  }
}