using System.Collections.Generic;
using Logger;
using UnityEngine;

public sealed class Ecosystem : MonoBehaviour
{
  private DataLogger _logger;
  
  private void Start()
  {
    _logger = DataLogger.Instance;
    _logger.InitializeLogging();
    _logger.Snapshot(0, GetAllAnimals());
  }

  private IList<Animal> GetAllAnimals()
  {
    // TODO: This should DEFINITELY be changed so that the ecosystem has a list of animals which it adds to when instancing the animals.
    var animals = FindObjectsOfType<Animal>();
    return animals;
  }
}