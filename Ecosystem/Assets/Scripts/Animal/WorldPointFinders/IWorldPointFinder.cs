using UnityEngine;

namespace Animal.WorldPointFinders
{
  public interface IWorldPointFinder
  { 
     MonoBehaviour GetRandomWalkablePoint(AbstractAnimal animal);
  }
}