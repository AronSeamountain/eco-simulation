using System.Collections.Generic;
using Animal;
using Foods.Plants;
using Logger;
using Logger.ConcreteLogger;
using Pools;
using UI;
using UI.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using ILogger = Logger.ILogger;

namespace Core
{
  public sealed class EntityManager : MonoBehaviour, IInspectable
  {
    public delegate void DayTick();

    public delegate void Tick();

    /// <summary>
    ///   The amount of time that a "unit" is in.
    /// </summary>
    public static float HoursInRealSeconds = 0.5f;

    private const float HoursPerDay = 24;
    public static int InitialWolves = 100;
    public static int InitialRabbits = 100;
    public static int InitialPlants = 100;
    [SerializeField] private GameObject rabbitPrefab;
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private GameObject plantPrefab;
    [SerializeField] private bool log;
    private AnimalPool _animalPool;
    private float _hoursPassed;
    private float _hourTicker;
    private ILogger _logger;
    public DayTick DayTickListeners;
    public Tick HourTickListeners;
    private int plantCount;
    public IList<AbstractAnimal> Animals { get; private set; }
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
      {
        DayTickListeners += plant.DayTick;
        HourTickListeners += plant.HourTick;
      }

      // Logger
      _logger = new MultiLogger(
        DetailedIndividualLogger.Instance,
        OverviewLogger.Instance
      );
    }

    private void Update()
    {
      UpdateTick();
    }

    public IEnumerable<AbstractProperty> GetProperties()
    {
      return PropertiesFactory.Create(this);
    }

    public void ShowGizmos(bool show)
    {
    }

    private void OnChildSpawned(AbstractAnimal child, AbstractAnimal parent)
    {
      CountAnimal(child, true);

      ObserveAnimal(child, true);
    }

    /// <summary>
    ///   Updates the count variable for the matching animal type. Increases it in case of an addition of animals, decreases on
    ///   death (not added).
    /// </summary>
    /// <param name="animal">The animal to count.</param>
    /// <param name="added">Whether the animal was added or died.</param>
    private void CountAnimal(AbstractAnimal animal, bool added)
    {
      var toAdd = added ? 1 : -1;
      switch (animal)
      {
        case Herbivore _:
          HerbivoreCount += toAdd;
          break;
        case Carnivore _:
          CarnivoreCount += toAdd;
          break;
      }
    }

    /// <summary>
    ///   Spawns animals and adds them to the list of animals.
    /// </summary>
    private void SpawnAndAddInitialAnimals()
    {
      SpawnAndAddGeneric(InitialRabbits, rabbitPrefab, Animals);
      HerbivoreCount += InitialRabbits;

      SpawnAndAddGeneric(InitialWolves, wolfPrefab, Animals);
      CarnivoreCount += InitialWolves;
    }

    private void SpawnAnimalSpecie(int amount, AnimalSpecies animalSpecies)
    {
      for (var i = 0; i < amount; i++)
      {
        var animal = _animalPool.Get(animalSpecies);
        Place(animal);
        Animals.Add(animal);
      }
    }

    /// <summary>
    ///   Spawns plants and adds them to the list of animals.
    /// </summary>
    private void SpawnAndAddInitialPlants()
    {
      SpawnAndAddGeneric(InitialPlants, plantPrefab, Plants);
    }

    private void SpawnAndAddGeneric<T>(int amount, GameObject prefab, ICollection<T> list = null)
      where T : MonoBehaviour
    {
      for (var i = 0; i < amount; i++)
      {
        var instance = Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<T>();
        Place(instance);
        list?.Add(instance);
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
      HourTickListeners += animal.HourTick;
      DayTickListeners += animal.DayTick;
      animal.ChildSpawnedListeners += OnChildSpawned;
      animal.DiedListeners += OnAnimalDied;
      animal.DecayedListeners += UnobserveAnimal;
    }

    private void UnobserveAnimal(AbstractAnimal animal)
    {
      Animals.Remove(animal);
      HourTickListeners -= animal.HourTick;
      DayTickListeners -= animal.DayTick;
      animal.ChildSpawnedListeners -= OnChildSpawned;
      animal.DiedListeners -= OnAnimalDied;
      animal.DecayedListeners -= UnobserveAnimal;
      
    }

    private void OnAnimalDied(AbstractAnimal animal)
    {
      CountAnimal(animal, false);
    }

    private void UpdateTick()
    {
      _hourTicker += Time.deltaTime;

      if (_hourTicker >= HoursInRealSeconds)
      {
        _hourTicker = 0;
        _hoursPassed++;
        HourTickListeners?.Invoke();

        if (_hoursPassed >= HoursPerDay)
        {
          _hoursPassed = 0;
          Days++;

          DayTickListeners?.Invoke();
          if (log)
          {
            _logger.Snapshot(this);
            _logger.Persist();
          }
        }
      }
    }
  }
}