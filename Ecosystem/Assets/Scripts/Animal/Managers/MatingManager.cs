using Animal.Sensor;
using UnityEngine;

namespace Animal.Managers
{
  public class MatingManager : MonoBehaviour
  {
    public delegate void AnimalFound(AbstractAnimal animal);

    [SerializeField] private Vision vision;
    [SerializeField] private Hearing hearing;
    private Gender _gender;
    public AnimalFound MateListeners;

    private void Start()
    {
      vision.AnimalFoundListeners += OnAnimalFound;
      hearing.AnimalHeardListeners += OnAnimalFound;
    }

    public void SetGender(Gender gender)
    {
      _gender = gender;
    }
    private void OnAnimalFound(AbstractAnimal animal)
    {
      if (animal == null) return;

      if (animal.Gender == _gender) return;

      MateListeners?.Invoke(animal);
    }
  }
}