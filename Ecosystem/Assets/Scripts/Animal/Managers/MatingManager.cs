using UnityEngine;

namespace Animal.Managers
{
  public class MatingManager: MonoBehaviour
  {
    [SerializeField] private VisualDetector visualDetector;
    public delegate void AnimalFound(AbstractAnimal animal);

    public AnimalFound MateListeners;
  
    private void Start()
    {
      visualDetector.AnimalFoundListeners += OnAnimalFound;
    }

    private void OnAnimalFound(AbstractAnimal animal)
    {
      if (animal == null) return;

      if (animal.Gender == Gender.Male) return;
    
      if(MateListeners!= null) MateListeners.Invoke(animal);
    }
  }
}