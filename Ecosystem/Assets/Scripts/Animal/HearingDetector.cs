﻿using UnityEngine;

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
      listeningArea = gameObject.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.GetComponent<AbstractAnimal>() is AbstractAnimal animal &&
          animal.transform.position != transform.parent.position)
      {
        AnimalHeardListeners?.Invoke(animal);
        listeningArea.material.SetColor("_Color", Color.blue);
      }
    }
    private void OnTriggerExit(Collider other)
    {
      if (other.GetComponent<AbstractAnimal>() is AbstractAnimal animal &&
          animal.transform.position != transform.parent.position)
      {
        listeningArea.material.SetColor("_Color", Color.white);
      }
    }
  }
}