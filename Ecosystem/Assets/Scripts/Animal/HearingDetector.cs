using UnityEngine;

namespace Animal
{
  /// <summary>
  ///   Triggers on sounds from other animals
  /// </summary>
  public sealed class HearingDetector : MonoBehaviour
  {
    /// <summary>
    ///   Is invoked when another animal is heard.
    /// </summary>
    /// <param name="animal"></param>
    public delegate void AnimalHeard(AbstractAnimal animal);

    [SerializeField] private Renderer listeningArea;
    private int _radius;
    public AnimalHeard AnimalHeardListeners;

    private int Radius
    {
      get => _radius;
      set => _radius = Mathf.Clamp(value, 0, int.MaxValue);
    }

    private void Start()
    {
      Radius = 12;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.GetComponent<AbstractAnimal>() is AbstractAnimal animal && NotSelf(animal))
      {
        AnimalHeardListeners?.Invoke(animal);
        IndicateSomethingInside(true);
      }
    }

    private void OnTriggerExit(Collider other)
    {
      if (other.GetComponent<AbstractAnimal>() is AbstractAnimal animal && NotSelf(animal))
        IndicateSomethingInside(false);
    }

    private bool NotSelf(Component animal)
    {
      return animal.transform.position != transform.parent.position;
    }

    private void IndicateSomethingInside(bool canHear)
    {
      var color = canHear ? Color.blue : Color.white;
      listeningArea.material.SetColor("_Color", color);
    }
  }
}