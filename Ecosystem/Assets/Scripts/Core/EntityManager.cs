using System.Collections.Generic;
using Animal;
using Foods.Plants;
using Logger;
using Pools;
using UI;
using UI.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Core
{
  public sealed class EntityManager : MonoBehaviour, IInspectable
  {
    public delegate void DayTick();

    public delegate void Tick();

    /// <summary>
    ///   The amount of time that a "unit" is in.
    /// </summary>
    private const float UnitTimeSeconds = 0.5f;

    private const float UnitsPerDay = 2;
    [SerializeField] private int initialAnimals = 1;
    [SerializeField] private int initialPlants = 4;
    [SerializeField] private GameObject plantPrefab;
    [SerializeField] private bool log;
    private AnimalPool _animalPool;
    private DataLogger _logger;
    private float _unitsPassed;
    private float _unitTicker;
    private int animalCount = 0;
    public DayTick DayTickListeners;
    private int plantCount;
    public Tick TickListeners;
    private IList<AbstractAnimal> Animals { get; set; }
    public int Days { get; private set; }
    public IList<Plant> Plants { get; private set; }
    public int HerbivoreCount { get; private set; }
    public int CarnivoreCount { get; private set; }

    private void Awake()
    {
      _animalPool = AnimalPool.SharedInstance;

      // Lists
      Animals = new List<AbstractAnimal>();
      SpawnAndAddInitialAnimals();
      Plants = new List<Plant>();
      SpawnAndAddInitialPlants();

      foreach (var animal in Animals)
        ObserveAnimal(animal, false);

      foreach (var plant in Plants)
        DayTickListeners += plant.DayTick;

      // Logger
      _logger = DataLogger.Instance;
      _logger.InitializeLogging();
    }

    private void Update()
    {
      UpdateTick();
    }

    public IList<AbstractProperty> GetStats(bool getStats)
    {
      return PropertiesFactory.Create(this);
    }

    private void OnChildSpawned(AbstractAnimal child, AbstractAnimal parent)
    {
      switch (child)
      {
        case Herbivore _:
          HerbivoreCount++;
          break;
        case Carnivore _:
          CarnivoreCount++;
          break;
      }

      ObserveAnimal(child, true);
    }

    /// <summary>
    ///   Spawns animals and adds them to the list of animals.
    /// </summary>
    private void SpawnAndAddInitialAnimals()
    {
      SpawnAnimalSpecie(initialAnimals, AnimalSpecie.Rabbit);
      HerbivoreCount += initialAnimals;

      SpawnAnimalSpecie(initialAnimals, AnimalSpecie.Wolf);
      HerbivoreCount += initialAnimals;
    }

    private void SpawnAnimalSpecie(int amount, AnimalSpecie animalSpecie)
    {
      for (var i = 0; i < initialPlants; i++)
      {
        var animal = _animalPool.Get(animalSpecie);
        Place(animal);
        Animals.Add(animal);
      }
    }

    /// <summary>
    ///   Spawns plants and adds them to the list of animals.
    /// </summary>
    private void SpawnAndAddInitialPlants()
    {
      SpawnAndAddGeneric(initialPlants, plantPrefab, Plants);
    }

    private void SpawnAndAddGeneric<T>(int amount, GameObject prefab, ICollection<T> list) where T : MonoBehaviour
    {
      for (var i = 0; i < amount; i++)
      {
        var instance = Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<T>();
        Place(instance);
        list.Add(instance);
      }
    }

    private void Place<T>(T instance) where T : MonoBehaviour
    {
      var coord = NavMeshUtil.GetRandomLocation();
      var foundPointOnNavMesh = NavMesh.SamplePosition(coord, out var hit, 50, -1);

      if (!foundPointOnNavMesh)
        QuitApplication("Could not find a position on the nav mesh when placing a game object");

      if (hit.position == Vector3.zero)
        QuitApplication("Attempted to place game object a non nav mesh place");

      if (instance.TryGetComponent(out NavMeshAgent agent))
      {
        if (!agent.Warp(hit.position))
          QuitApplication("Could not warp agent to nav mesh");
      }
      else
      {
        instance.transform.position = hit.position;
      }
    }

    private void QuitApplication(string message = "oof")
    {
      Debug.LogError(message);

#if UNITY_EDITOR
      EditorApplication.isPlaying = false;
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
    private void ObserveAnimal(AbstractAnimal animal, bool addToList)
    {
      if (!animal) return;
      if (addToList) Animals.Add(animal);
      TickListeners += animal.Tick;
      DayTickListeners += animal.DayTick;
      animal.ChildSpawnedListeners += OnChildSpawned;
    }

    private void UpdateTick()
    {
      _unitTicker += Time.deltaTime;

      if (_unitTicker >= UnitTimeSeconds)
      {
        _unitTicker = 0;
        _unitsPassed++;
        TickListeners?.Invoke();

        if (_unitsPassed >= UnitsPerDay)
        {
          _unitsPassed = 0;
          Days++;

          DayTickListeners?.Invoke();
          if (log) _logger.Snapshot(Days, Animals);
        }
      }
    }
  }
}