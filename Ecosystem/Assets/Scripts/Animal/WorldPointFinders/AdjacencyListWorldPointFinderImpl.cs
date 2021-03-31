using Core;
using UnityEngine;
using Utils;

namespace Animal.WorldPointFinders
{
  public class AdjacencyListWorldPointFinderImpl: IWorldPointFinder
  {
    public MonoBehaviour getRandomWalkablePoint(AbstractAnimal _animal)
    {
      return NavMeshUtil.getRandomWalkablePointAdjacencyList(_animal.gameObject.transform.position);
    }
  }
}