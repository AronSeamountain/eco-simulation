using System.Collections.Generic;
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
  private IList<Animal> _animals;
  private int _days;
  private DataLogger _logger;
  private Tick _tickListeners;
  private float _unitsPassed;
  private float _unitTicker;

  private void Start()
  {
    _animals = new List<Animal>();
    SpawnAndAddInitialAnimals();

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

  private void OnChildSpawned(Animal child)
  {
    ObserveAnimal(child);
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
      ObserveAnimal(animal);
    }
  }

  /// <summary>
  ///   Adds the animal to a list of existing animals. Listens to ChildSpawned events. Does nothing if the animal is null.
  /// </summary>
  /// <param name="animal">The animal to observe.</param>
  private void ObserveAnimal(Animal animal)
  {
    if (!animal) return;
    _animals.Add(animal);
    animal.ChildSpawnedListeners += OnChildSpawned;
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