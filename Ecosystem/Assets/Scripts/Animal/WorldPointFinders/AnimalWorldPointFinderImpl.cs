using Core;
using UnityEngine;
using Utils;

namespace Animal
{
  public class AnimalWorldPointFinderImpl : IWorldPointFinder
  {
    public MonoBehaviour getRandomWalkablePoint(AbstractAnimal _animal)
    {
      return NavMeshUtil.getRandomWalkablePoint();
    }
  }
}