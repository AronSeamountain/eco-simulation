using UnityEngine;

public class MatingManager: MonoBehaviour
{
  [SerializeField] private VisualDetector visualDetector;
  public delegate void AnimalFound(Animal animal);

  public AnimalFound MateListeners;
  
  private void Start()
  {
    visualDetector.AnimalFoundListeners += OnAnimalFound;
  }

  private void OnAnimalFound(Animal animal)
  {
    if (animal == null) return;

    if (animal.GetGender() == Gender.Male) return;
    
    if(MateListeners!= null) MateListeners.Invoke(animal);
  }
}