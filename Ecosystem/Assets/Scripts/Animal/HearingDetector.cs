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

    public delegate void AnimalLeftHearing(AbstractAnimal animal);

    [SerializeField] private Transform listeningArea;
    private Renderer _component;
    private int _radius;
    public AnimalHeard AnimalHeardListeners;
    public AnimalLeftHearing AnimalLeftHearingListeners;

    private int Radius
    {
      get => _radius;
      set => _radius = Mathf.Clamp(value, 0, int.MaxValue);
    }

    private void Start()
    {
      Radius = 12;
      _component = listeningArea.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.GetComponent<AbstractAnimal>() is AbstractAnimal animal &&
          animal.transform.position != transform.parent.position)
      {
        AnimalHeardListeners?.Invoke(animal);
        _component.material.SetColor("_Color", Color.blue);
      }
    }
    private void OnTriggerExit(Collider other)
    {
      if (other.GetComponent<AbstractAnimal>() is AbstractAnimal animal &&
          animal.transform.position != transform.parent.position)
      {
        _component.material.SetColor("_Color", Color.white);
      }
    }
  }
}