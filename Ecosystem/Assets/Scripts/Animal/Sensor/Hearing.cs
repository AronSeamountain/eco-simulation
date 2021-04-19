using UnityEngine;

namespace Animal.Sensor
{
  /// <summary>
  ///   Triggers on sounds from other animals
  /// </summary>
  public sealed class Hearing : MonoBehaviour
  {
    /// <summary>
    ///   Is invoked when another animal is heard.
    /// </summary>
    /// <param name="animal"></param>
    public delegate void AnimalHeard(AbstractAnimal animal);

    private int _maxRadius = 24;
    private int _radius;
    public AnimalHeard AnimalHeardListeners;

    private int Radius
    {
      get => _radius;
      set
      {
        _radius = Mathf.Clamp(value, 0, int.MaxValue);
        updateScale();
      }
    }

    private void updateScale()
    {
      transform.localScale = new Vector3(Radius, Radius, Radius);
    }

    private void Start()
    {
      updateScale();
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.GetComponent<AbstractAnimal>() is AbstractAnimal animal && NotSelf(animal) && animal.Alive)
        AnimalHeardListeners?.Invoke(animal);
    }

    private bool NotSelf(Component animal)
    {
      return animal.transform.position != transform.parent.position;
    }

    public void SetSizePercentage(float percentage)
    {
      Radius = (int) (_maxRadius * percentage);
    }
  }
}