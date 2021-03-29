using Core;
using UnityEngine;
using Utils;

namespace Animal
{
  public class AnimalWorldPointFinderImpl : IWorldPointFinder
  {
    public MonoBehaviour getRandomWalkablePoint()
    {
      return NavMeshUtil.getRandomWalkablePoint();
    }
  }
}