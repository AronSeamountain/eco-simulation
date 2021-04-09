using Core;
using UnityEngine;
using Utils;

namespace Animal.WorldPointFinders
{
  public class AdjacencyListWorldPointFinderImpl : IWorldPointFinder
  {
    public MonoBehaviour GetRandomWalkablePoint(AbstractAnimal _animal)
    {
      return NavMeshUtil.GetRandomWalkablePointAdjacencyList(_animal.gameObject.transform.position);
    }
  }
}