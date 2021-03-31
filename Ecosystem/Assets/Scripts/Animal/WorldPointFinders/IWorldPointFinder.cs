using Animal;
using UnityEngine;

namespace Core
{
  public interface IWorldPointFinder
  { 
     MonoBehaviour getRandomWalkablePoint(AbstractAnimal _animal);
  }
}