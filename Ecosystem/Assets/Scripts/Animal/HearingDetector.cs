using UnityEngine;

namespace Animal
{
  /// <summary>
  ///   Triggers on sounds from other animals
  /// </summary>
  public sealed class HearingDetector : MonoBehaviour
  {
    /// <summary>
    /// Is invoked when another animal is heard.
    /// </summary>
    /// <param name="animal"></param>
    public delegate void AnimalHeard(AbstractAnimal animal);

    [SerializeField] private Transform hearingTransform;
    private int _radius;
    public AnimalHeard AnimalHeardListeners;
    
    private int Radius
    {
      get => _radius;
      set => _radius = Mathf.Clamp(value, 0, int.MaxValue);
    }

    private void Start()
    {
      _radius = 8;
    }
    
    

  } 
  
}
