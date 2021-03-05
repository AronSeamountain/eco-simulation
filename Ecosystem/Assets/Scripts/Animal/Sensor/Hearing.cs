﻿using UnityEngine;

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
      if (other.GetComponent<AbstractAnimal>() is AbstractAnimal animal && NotSelf(animal) && animal.Alive)
        AnimalHeardListeners?.Invoke(animal);
    }

    private bool NotSelf(Component animal)
    {
      return animal.transform.position != transform.parent.position;
    }
  }
}