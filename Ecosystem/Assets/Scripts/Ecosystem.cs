﻿using System.Collections.Generic;
using Logger;
using UnityEngine;

public sealed class Ecosystem : MonoBehaviour
{
  /// <summary>
  ///   The amount of time that a "unit" is in.
  /// </summary>
  private const float UnitTimeSeconds = 0.5f;

  private const float UnitsPerDay = 2;
  [SerializeField] private int initialEntities = 1;
  [SerializeField] private GameObject animalPrefab;
  [SerializeField] private GameObject plantPrefab;
  private IList<Animal> _animals;
  private IList<Plant> _plants;
  private int _days;
  private DataLogger _logger;
  private Tick _tickListeners;
  private float _unitsPassed;
  private float _unitTicker;

  private void Start()
  {
    _animals = new List<Animal>();
    SpawnAndAddInitialAnimals();
    
    SpawnAndAddInitialPlants();

    _logger = DataLogger.Instance;
    _logger.InitializeLogging();

    foreach (var animal in _animals)
      _tickListeners += animal.Tick;

    _logger.Snapshot(0, _animals);
  }

  private void Update()
  {
    UpdateTick();
  }

  /// <summary>
  ///   Spawns animals and adds them to the list of animals.
  /// </summary>
  private void SpawnAndAddInitialAnimals()
  {
    const float spawnSquareHalfWidth = 10f;
    for (var i = 0; i < initialEntities; i++)
    {
      var randomPos = new Vector3(
        Random.Range(-spawnSquareHalfWidth, spawnSquareHalfWidth),
        1.5f,
        Random.Range(-spawnSquareHalfWidth, spawnSquareHalfWidth)
      );
      var animal = Instantiate(animalPrefab, randomPos, Quaternion.identity).GetComponent<Animal>();
      _animals.Add(animal);
    }
  }

  private void SpawnAndAddInitialPlants()//TODO fix
  {
    const float spawnSquareHalfWidth = 10f;
    for (var i = 0; i < initialEntities; i++)
    {
      var randomPos = new Vector3(
        Random.Range(-spawnSquareHalfWidth, spawnSquareHalfWidth),
        1.5f,
        Random.Range(-spawnSquareHalfWidth, spawnSquareHalfWidth)
      );
      var plant = Instantiate(plantPrefab, randomPos, Quaternion.identity).GetComponent<Plant>();
      _plants.Add(plant);
    }
  }
  private void UpdateTick()
  {
    _unitTicker += Time.deltaTime;

    if (_unitTicker >= UnitTimeSeconds)
    {
      _unitTicker = 0;
      _unitsPassed++;
      _tickListeners?.Invoke();

      if (_unitsPassed >= UnitsPerDay)
      {
        _unitsPassed = 0;
        _days++;

        _logger.Snapshot(_days, _animals);
      }
    }
  }

  private delegate void Tick();
}