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
    [SerializeField] private GameObject objectPrefab;
    private IDictionary<AnimalSpecie, Stack<AbstractAnimal>> _speciePoolMapping;

    private void Awake()
    {
      _speciePoolMapping = new Dictionary<AnimalSpecie, Stack<AbstractAnimal>>();

      foreach (AnimalSpecie specie in Enum.GetValues(typeof(AnimalSpecie)))
        _speciePoolMapping[specie] = new Stack<AbstractAnimal>();

      SharedInstance = this;
    }

    public AbstractAnimal Get(AnimalSpecie animalSpecie)
    {
      var stack = GetStack(animalSpecie);
      return stack.Count > 0 ? stack.Pop() : CreateObject();
    }

    private Stack<AbstractAnimal> GetStack(AnimalSpecie animalSpecie)
    {
      return _speciePoolMapping[animalSpecie];
    }

    private AbstractAnimal CreateObject()
    {
      var instance = Instantiate(objectPrefab, Vector3.zero, Quaternion.identity).GetComponent<AbstractAnimal>();
      return instance;
    }

    public void Pool(AbstractAnimal animal, AnimalSpecie animalSpecie)
    {
      var stack = GetStack(animalSpecie);

      if (stack.Count < AmountToPool)
        stack.Push(animal);
      else
        Destroy(animal);
    }
  }
}