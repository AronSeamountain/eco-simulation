using Animal.WorldPointFinders;
using Core;
using UnityEngine;
using Utils;

namespace Animal.Sensor
{
  public class AnimalWorldPointFinderMatrixImpl : IWorldPointFinder
  {
    public MonoBehaviour GetRandomWalkablePoint(AbstractAnimal _animal)
    {
      return NavMeshUtil.GetRandomWalkablePointInMatrix(_animal.gameObject.transform.position);
    }
  }
}