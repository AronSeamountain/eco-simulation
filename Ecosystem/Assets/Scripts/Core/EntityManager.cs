using System;
using System.Collections;
using System.Collections.Generic;
using Animal;
using Foods.Plants;
using Logger;
using Logger.ConcreteLogger;
using Menu;
using Pools;
using UI;
using UI.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Utils;
using ILogger = Logger.ILogger;
using Random = UnityEngine.Random;

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

    public static void Restart()
    {
      HoursInRealSeconds = 0.5f;
      InitialWolves = 25;
      InitialRabbits = 100;
      InitialPlants = 100;
      PerformanceModeMenuOverride = true;
      OverlappableAnimalsMenuOverride = false;
      LogMenuOverride = true;
      OptionsMenu.World = "LargeWorld";
    }

    private const float HoursPerDay = 24;
    public static int InitialWolves = 25;
    public static int InitialRabbits = 100;
    public static int InitialPlants = 100;
    [SerializeField] private GameObject plantPrefab;
    [SerializeField] private GameObject walkablePointPrefab;
    [SerializeField] private bool log;
    [SerializeField] private bool performanceMode;
    [SerializeField] private bool overlappableAnimals;
    private float _hoursPassed;
    private float _hourTicker;
    private ILogger _logger;
    public DayTick DayTickListeners;
    public Tick HourTickListeners;
    private int _plantCount;
    private const int DaysBetweenLogs = 3;
    private int _daysSinceLastLog;

    public IList<AbstractAnimal> Animals { get; private set; }
    public int Days { get; private set; }
    public IList<Plant> Plants { get; private set; }
    public int HerbivoreCount { get; private set; }
    public int CarnivoreCount { get; private set; }
    public FpsDelegate FpsDelegate { get; private set; }
    public static bool PerformanceModeMenuOverride = true;
    public static bool PerformanceMode;
    public static bool OverlappableAnimalsMenuOverride = false;
    public static bool LogMenuOverride = true;
    public bool Log { get; private set; }


    private void Awake()
    {
      SpawnAndAddWalkablePoints();
      Log = log || LogMenuOverride;
      PerformanceMode = performanceMode || PerformanceModeMenuOverride;

      AnimalPool.OverlappableAnimals = overlappableAnimals || OverlappableAnimalsMenuOverride;

      FpsDelegate = new FpsDelegate();

      // Lists
      Animals = new List<AbstractAnimal>();
      Plants = new List<Plant>();
      var sceneName = SceneManager.GetActiveScene().name;
      if (sceneName.Equals("EvadeScene"))
      {
        InitEvadeScene();
      }
      else
      {
        SpawnAndAddInitialAnimals();
        SpawnAndAddInitialPlants();
      }

      SpawnAndAddWalkablePoints();
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
        new OverviewLogger(),
        new FpsLogger()
      );

      _logger.Clear();
    }

    private void Update()
    {
      UpdateTick();
      if (Log) FpsDelegate.FramePassed();
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
      child.MomUuid = parent.Uuid;
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
    ///   Creates the walkable points which the animals will look for
    /// </summary>
    public void SpawnAndAddWalkablePoints()
    {
      List<MonoBehaviour>[,] matrix = WorldMatrix.InitMatrix();
      PopulateWorldWithWalkablePoints(matrix);
      WorldMatrix.AddWalkablePointsToMatrix(matrix);
      WorldMatrix.PopulateAdjacencyList(matrix);
    }

    private void PopulateWorldWithWalkablePoints(List<MonoBehaviour>[,] matrix)
    {
      for (int i = 0; i < matrix.GetLength(0); i++)
      {
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
          SpawnAndAddGeneric(WorldMatrix.WalkablePointsAmountPerBox, walkablePointPrefab,
            i * WorldMatrix.WalkableMatrixBoxSize, (i + 1) * WorldMatrix.WalkableMatrixBoxSize,
            j * WorldMatrix.WalkableMatrixBoxSize, (j + 1) * WorldMatrix.WalkableMatrixBoxSize,
            WorldMatrix.WalkablePoints);
        }
      }
    }

    /// <summary>
    ///   Spawns animals and adds them to the list of animals.
    /// </summary>
    private void SpawnAndAddInitialAnimals()
    {
      SpawnAndAddSpecies(InitialRabbits, AnimalSpecies.Rabbit, Animals);
      HerbivoreCount += InitialRabbits;


      SpawnAndAddSpecies(InitialWolves, AnimalSpecies.Wolf, Animals);
      CarnivoreCount += InitialWolves;

      InitAnimalGameObejcts();
    }

    private void InitAnimalGameObejcts()
    {
      for (int i = 0; i < Animals.Count; i++)
      {
        Animals[i].ResetGameObject();
      }
    }

    /// <summary>
    ///   Spawns plants and adds them to the list of animals.
    /// </summary>
    private void SpawnAndAddInitialPlants()
    {
      SpawnAndAddPrefab(InitialPlants, plantPrefab, Plants);
    }

    private void SpawnAndAddSpecies<T>(int amount, AnimalSpecies species, ICollection<T> list = null)
      where T : MonoBehaviour
    {
      var pool = AnimalPool.SharedInstance;
      for (var i = 0; i < amount; i++)
      {
        var instance = pool.Get(species) as T;
        Place(instance);
        list?.Add(instance);
      }
    }

    private void SpawnAndAddPrefab<T>(int amount, GameObject prefab, ICollection<T> list = null)
      where T : MonoBehaviour
    {
      for (var i = 0; i < amount; i++)
      {
        var instance = Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<T>();


        Place(instance);
        list?.Add(instance);
      }
    }

    private void SpawnAndAddGeneric<T>(int amount, GameObject prefab, int xMin, int xMax, int zMin, int zMax,
      ICollection<T> list = null)
      where T : MonoBehaviour
    {
      for (var i = 0; i < amount; i++)
      {
        var x = Random.Range(xMin, xMax);
        var z = Random.Range(zMin, zMax);
        var vector = new Vector3(x, 0, z);
        var instance = Instantiate(prefab, vector, Quaternion.identity).GetComponent<T>();

        Place(instance, vector);
        list?.Add(instance);
      }
    }

    private void Place<T>(T instance) where T : MonoBehaviour
    {
      const int maxOffset = 20;

      var points = WorldMatrix.WalkablePoints;
      var walkablePoint = points.PickRandom().gameObject.transform.position;

      var spawnPoint = walkablePoint + Random.onUnitSphere * Random.Range(-maxOffset, maxOffset);
      Place(instance, spawnPoint);
    }

    private void Place<T>(T instance, Vector3 v) where T : MonoBehaviour
    {
      var foundPointOnNavMesh = NavMesh.SamplePosition(v, out var hit, 300, -1);

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

          if (Log)
          {
            _daysSinceLastLog++;

            _logger.Snapshot(this);
            FpsDelegate.Reset();

            if (_daysSinceLastLog >= DaysBetweenLogs)
            {
              _daysSinceLastLog = 0;
              _logger.Persist();
            }
          }
        }
      }
    }

    private void InitEvadeScene()
    {
      var pool = AnimalPool.SharedInstance;

      var wolf = pool.Get(AnimalSpecies.Wolf);
      var vector = new Vector3(5, 0, 5);
      Place(wolf, vector);
      Animals?.Add(wolf);

      var rabbit = pool.Get(AnimalSpecies.Rabbit);
      var vector2 = new Vector3(5, 0, 10);
      Place(rabbit, vector2);
      Animals?.Add(rabbit);

      InitAnimalGameObejcts();
      wolf.GetNourishmentDelegate().Saturation = 0;
      rabbit.SpeedModifier = 1;
      wolf.SpeedModifier = 1.3f;
      GeneralTestInit();
    }

    private void GeneralTestInit()
    {
      PerformanceMode = false;
      Log = false;
      foreach (var animal in Animals)
      {
        if (animal.Species == AnimalSpecies.Rabbit)
          HerbivoreCount++;
        if (animal.Species == AnimalSpecies.Wolf)
          CarnivoreCount++;
      }
    }
  }
}