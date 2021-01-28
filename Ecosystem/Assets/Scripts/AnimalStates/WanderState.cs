using UnityEngine;

namespace AnimalStates
{
  public class WanderState : IState
  {
    private Vector3 _nextPosition;
    
    public void Enter(Animal animal)
    {
      var position = animal.transform.position;
      var randomPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
      var newLocation = position + randomPos;
      _nextPosition = newLocation;
      animal.Goto(_nextPosition);
    }

    public void Exit(Animal animal)
    {
    }

    public IState Execute(Animal animal)
    {
      if (!animal.IsMoving)
      {
        Enter(animal);
      }
      return this;
    }
  }
}