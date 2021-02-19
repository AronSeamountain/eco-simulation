using System.Collections.Generic;
using Foods.Plants;
using Logger;
using UnityEngine;
using UnityEngine.AI;
using Utils;

public sealed class EntityManager : MonoBehaviour
{
  /// <summary>
  ///   The amount of time that a "unit" is in.
  /// </summary>
  private const float UnitTimeSeconds = 0.5f;

  private const float UnitsPerDay = 2;
  [SerializeField] private int initialAnimals = 1;
  [SerializeField] private int initialPlants = 4;
  [SerializeField] private GameObject animalPrefab;
  [SerializeField] private GameObject plantPrefab;
  [SerializeField] private Transform spawnLocation;
  private IList<Animal> _animals;
  private int _days;
  private DayTick _dayTickListeners;
  private DataLogger _logger;
  private IList<Plant> _plants;
  private Vector3 _spawnLocationVector3;
  private Tick _tickListeners;
  private float _unitsPassed;
  private float _unitTicker;

  private void Start()
  {
    _spawnLocationVector3 = spawnLocation.position;

    // Lists
    _animals = new List<Animal>();
    SpawnAndAddInitialAnimals();
    _plants = new List<Plant>();
    SpawnAndAddInitialPlants();

    foreach (var animal in _animals)
      ObserveAnimal(animal, false);

    foreach (var plant in _plants)
      _dayTickListeners += plant.DayTick;

    // Logger
    _logger = DataLogger.Instance;
    _logger.InitializeLogging();
    _logger.Snapshot(0, _animals);
  }

  private void Update()
  {
    UpdateTick();
  }

  private void OnChildSpawned(Animal child)
  {
    ObserveAnimal(child, true);
  }

  /// <summary>
  ///   Spawns animals and adds them to the list of animals.
  /// </summary>
  private void SpawnAndAddInitialAnimals()
  {
    SpawnAndAddGeneric(initialAnimals, animalPrefab, _animals);
  }

  /// <summary>
  ///   Spawns plants and adds them to the list of animals.
  /// </summary>
  private void SpawnAndAddInitialPlants()
  {
    SpawnAndAddGeneric(initialPlants, plantPrefab, _plants);
  }

  private void SpawnAndAddGeneric<T>(int amount, GameObject prefab, ICollection<T> list) where T : MonoBehaviour
  {
    for (var i = 0; i < amount; i++)
    {
      var instance = Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<T>();

      if (instance.TryGetComponent(out NavMeshAgent agent))
        PlaceNavAgent(agent);
      else
        PlaceSimple(instance);

      list.Add(instance);
    }
  }

  private void PlaceSimple<T>(T instance) where T : MonoBehaviour
  {
    const float spawnSquareHalfWidth = 30f;

    var offset = new Vector3(
      Random.Range(-spawnSquareHalfWidth, spawnSquareHalfWidth),
      100,
      Random.Range(-spawnSquareHalfWidth, spawnSquareHalfWidth)
    );

    var pos = _spawnLocationVector3 + offset;
    instance.transform.position = pos;
  }

  private void PlaceNavAgent(NavMeshAgent agent)
  {
    var coord = NavMeshUtil.GetRandomLocation();

    var foundPointOnNavMesh = NavMesh.SamplePosition(coord, out var hit, 50, -1);
    var successfullyWarped = agent.Warp(hit.position);

    if (!foundPointOnNavMesh || !successfullyWarped) // TODO: Test, assume that it works?
      QuitApplication("Could not find a position on the nav mesh");
  }

  private void QuitApplication(string message = "oof")
  {
    Debug.LogError(message);

#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
  }

  /// <summary>
  ///   Adds the animal to a list of existing animals. Listens to ChildSpawned events. Adds the animal to the tick events.
  ///   Does nothing if the animal is null.
  /// </summary>
  /// <param name="animal">The animal to observe.</param>
  /// <param name="addToList">Whether to add it to the list of animals.</param>
  private void ObserveAnimal(Animal animal, bool addToList)
  {
    if (!animal) return;
    if (addToList) _animals.Add(animal);
    _tickListeners += animal.Tick;
    _dayTickListeners += animal.DayTick;
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

        _dayTickListeners?.Invoke();
        _logger.Snapshot(_days, _animals);
      }
    }
  }

  private delegate void Tick();

  private delegate void DayTick();
}