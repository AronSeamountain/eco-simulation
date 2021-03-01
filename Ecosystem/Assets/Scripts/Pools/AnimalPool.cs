using System;
using System.Collections.Generic;
using Animal;
using UnityEngine;

namespace Pools
{
  public sealed class AnimalPool : MonoBehaviour
  {
    private const int AmountToPool = 10;
    public static AnimalPool SharedInstance;
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private GameObject rabbitPrefab;
    private IDictionary<AnimalSpecie, Stack<AbstractAnimal>> _speciePoolMapping;
    private IDictionary<AnimalSpecie, GameObject> _speciePrefabMapping;

    private void Awake()
    {
      _speciePoolMapping = new Dictionary<AnimalSpecie, Stack<AbstractAnimal>>();

      foreach (AnimalSpecie specie in Enum.GetValues(typeof(AnimalSpecie)))
        _speciePoolMapping[specie] = new Stack<AbstractAnimal>();

      _speciePrefabMapping = new Dictionary<AnimalSpecie, GameObject>
      {
        {AnimalSpecie.Wolf, wolfPrefab},
        {AnimalSpecie.Rabbit, rabbitPrefab}
      };

      SharedInstance = this;
    }

    public AbstractAnimal Get(AnimalSpecie specie)
    {
      var stack = GetStack(specie);
      return stack.Count > 0 ? stack.Pop() : CreateAnimal(specie);
    }

    private Stack<AbstractAnimal> GetStack(AnimalSpecie specie)
    {
      return _speciePoolMapping[specie];
    }

    private AbstractAnimal CreateAnimal(AnimalSpecie specie)
    {
      var prefab = _speciePrefabMapping[specie];
      return Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<AbstractAnimal>();
    }

    public void Pool(AbstractAnimal animal)
    {
      var stack = GetStack(animal.Specie);

      if (stack.Count < AmountToPool)
        stack.Push(animal);
      else
        Destroy(animal.gameObject);
    }
  }
}