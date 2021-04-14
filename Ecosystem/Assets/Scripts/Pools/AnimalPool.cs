using System;
using System.Collections.Generic;
using Animal;
using UnityEngine;
using UnityEngine.AI;

namespace Pools
{
  public sealed class AnimalPool : MonoBehaviour
  {
    private const int AmountToPool = 0;
    public static AnimalPool SharedInstance;
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private GameObject rabbitPrefab;
    private IDictionary<AnimalSpecies, Stack<AbstractAnimal>> _speciePoolMapping;
    private IDictionary<AnimalSpecies, GameObject> _speciePrefabMapping;
    public static bool OverlappableAnimals;

    private void Awake()
    {
      _speciePoolMapping = new Dictionary<AnimalSpecies, Stack<AbstractAnimal>>();

      foreach (AnimalSpecies specie in Enum.GetValues(typeof(AnimalSpecies)))
        _speciePoolMapping[specie] = new Stack<AbstractAnimal>();

      _speciePrefabMapping = new Dictionary<AnimalSpecies, GameObject>
      {
        {AnimalSpecies.Wolf, wolfPrefab},
        {AnimalSpecies.Rabbit, rabbitPrefab}
      };

      SharedInstance = this;
    }

    /// <summary>
    ///   Gets a pooled animal. The animal will have the active property set to false.
    /// </summary>
    /// <param name="species">The type of animal to retrieve.</param>
    /// <returns>The pooled animal.</returns>
    public AbstractAnimal Get(AnimalSpecies species)
    {
      var stack = GetStack(species);
      return stack.Count > 0 ? stack.Pop() : CreateAnimal(species);
    }

    private Stack<AbstractAnimal> GetStack(AnimalSpecies species)
    {
      return _speciePoolMapping[species];
    }

    /// <summary>
    ///   Creates a new animal that has collision avoidance according to the OverlappableAnimals field.
    /// </summary>
    /// <param name="species">The type of animal to create.</param>
    /// <returns>A animal instance.</returns>
    private AbstractAnimal CreateAnimal(AnimalSpecies species)
    {
      var prefab = _speciePrefabMapping[species];
      var instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);

      // Overlap
      var agent = instance.GetComponent<NavMeshAgent>();

      if (OverlappableAnimals)
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
      else
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;

      return instance.GetComponent<AbstractAnimal>();
    }

    public void Pool(AbstractAnimal animal)
    {
      var stack = GetStack(animal.Species);
      animal.gameObject.SetActive(false);

      if (stack.Count < AmountToPool)
        stack.Push(animal);
      else
        Destroy(animal.gameObject);
    }
  }
}