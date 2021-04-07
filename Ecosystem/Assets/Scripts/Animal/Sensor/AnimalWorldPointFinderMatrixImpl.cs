using Core;
using UnityEngine;
using Utils;

namespace Animal.Sensor
{
  public class AnimalWorldPointFinderMatrixImpl : IWorldPointFinder
  {
    public MonoBehaviour getRandomWalkablePoint(AbstractAnimal _animal)
    {
      return NavMeshUtil.getRandomWalkablePointInMatrix(_animal.gameObject.transform.position);
    }
  }
}