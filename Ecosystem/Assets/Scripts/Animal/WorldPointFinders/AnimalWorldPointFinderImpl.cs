using Animal.WorldPointFinders;
using Core;
using UnityEngine;
using Utils;

namespace Animal
{
  public class AnimalWorldPointFinderImpl : IWorldPointFinder
  {
    public MonoBehaviour GetRandomWalkablePoint(AbstractAnimal _animal)
    {
      return NavMeshUtil.GetRandomWalkablePoint();
    }
  }
}