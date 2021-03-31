using System;
using System.Collections;
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

    public static string World = "LargeWorld";
    private const float HoursPerDay = 24;
    public static int InitialWolves = 300;
    public static int InitialRabbits = 0;
    public static int InitialPlants = 600;
    private static int WalkablePointsAmountPerBox = 5;
    public static int amountOfBoxesPerMatrixLayer = 15;
    public static int WalkableMatrixBoxSize; //should not be set manually
    public static int WorldSize;
    [SerializeField] private GameObject rabbitPrefab;
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private GameObject plantPrefab;
    [SerializeField] private GameObject walkablePointPrefab;
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

    public IList<MonoBehaviour> WalkablePoints { get; private set; }

    private void Awake()
    {
      _animalPool = AnimalPool.SharedInstance;

      // Lists
      Animals = new List<AbstractAnimal>();
      SpawnAndAddInitialAnimals();
      Plants = new List<Plant>();
      SpawnAndAddInitialPlants();
     
      WalkablePoints = new List<MonoBehaviour>();
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
    ///   Creates the walkable points which the animals will look for
    /// </summary>
    private void SpawnAndAddWalkablePoints()
    {
      if (World.Equals("LargeWorld"))
      {
        WorldSize = 500;
      }
      else
      {
        WorldSize = 150;
      }
      List<MonoBehaviour>[,] matrix = InitMatrix();
      PopulateWorldWithWalkablePoints(matrix);
      AddWalkablePointsToMatrix(matrix);
      PopulateAdjacencyList(matrix);
     
    }
  
    private List<MonoBehaviour>[,]  InitMatrix()
    {
      WalkableMatrixBoxSize =(int) Math.Ceiling(WorldSize / (float) amountOfBoxesPerMatrixLayer);
      Debug.Log("WALK" + WalkableMatrixBoxSize);
      List<MonoBehaviour>[,] matrix = new List<MonoBehaviour>[amountOfBoxesPerMatrixLayer, amountOfBoxesPerMatrixLayer];
      for (int i = 0; i < matrix.GetLength(0); i++)
      {
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
          matrix[i, j] = new List<MonoBehaviour>();
        }
      }

      return matrix;
    }

    private void AddWalkablePointsToMatrix(List<MonoBehaviour>[,] matrix)
    {
      foreach (var wp in WalkablePoints)
      {
        int x = (int) Mathf.Floor(wp.gameObject.transform.position.x / WalkableMatrixBoxSize);
        int z = (int) Mathf.Floor(wp.gameObject.transform.position.z / WalkableMatrixBoxSize);

        matrix[x, z].Add(wp);
      }
      NavMeshUtil.WalkablePointMatrix = matrix;
    }
    private void PopulateWorldWithWalkablePoints( List<MonoBehaviour>[,] matrix)
    {
      for (int i = 0; i < matrix.GetLength(0); i++)
      {
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
          SpawnAndAddGeneric(WalkablePointsAmountPerBox, walkablePointPrefab, i * WalkableMatrixBoxSize, (i + 1) * WalkableMatrixBoxSize,
            j * WalkableMatrixBoxSize, (j + 1) * WalkableMatrixBoxSize, WalkablePoints);
        }
      }
      NavMeshUtil.WalkablePoints = WalkablePoints;
    }
    private void PopulateAdjacencyList(List<MonoBehaviour>[,] matrix)
    {
      IList<List<MonoBehaviour>> adjacencyList =
        new List<List<MonoBehaviour>>();


      for (int i = 0; i < matrix.GetLength(0); i++)
      {
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
          var tempList = new List<MonoBehaviour>();
          if (j > 0)
            foreach (var monoBehaviour in matrix[i, j - 1]) // adds all from matrix entry to the left
              tempList.Add(monoBehaviour);
          if (j < matrix.GetLength(1) - 1)
          {

            foreach (var monoBehaviour in matrix[i, j + 1])
            {
              // adds all from matrix entry to the right
              tempList.Add(monoBehaviour);
            }
          }
          
          if (i > 0)
            foreach (var monoBehaviour in matrix[i - 1, j]) // adds all from matrix entry above
              tempList.Add(monoBehaviour);

          if (i < matrix.GetLength(0) - 1)
            foreach (var monoBehaviour in matrix[i + 1, j]) // adds all from matrix entry below
              tempList.Add(monoBehaviour);
          
         
          foreach (var monoBehaviour in matrix[i, j]) // adds all from current matrix entry 
            tempList.Add(monoBehaviour);

          if(tempList.Count < 1) Debug.Log("You done fucked up");
          adjacencyList.Add(tempList);
        }
      }
      NavMeshUtil.WalkablePointAdjacencyList = adjacencyList;
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
    
    private void SpawnAndAddGeneric<T>(int amount, GameObject prefab,int xMin, int xMax, int zMin,int zMax, ICollection<T> list = null)
      where T : MonoBehaviour
    {
      for (var i = 0; i < amount; i++)
      {
        var x = Random.Range(xMin, xMax);
        var z = Random.Range(zMin, zMax);
        var vector = new Vector3(x, 0, z);
        var instance = Instantiate(prefab, vector, Quaternion.identity).GetComponent<T>();
        

        Place(instance);
        list?.Add(instance);
      }
    }

    private void Place<T>(T instance) where T : MonoBehaviour
    {
      var vector = NavMeshUtil.GetRandomLocation();
      Place(instance,vector);
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