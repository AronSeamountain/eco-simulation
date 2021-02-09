using UnityEngine;

namespace AnimalStates
{
  public sealed class DeadState : IState
  {
    public AnimalState GetStateEnum()
    {
      return AnimalState.Dead;
    }
    
    public void Enter(Animal animal)
    {
      animal.StopMoving(); // may have to be changed to something else?
    }
    
    public void Exit(Animal animal)
    {
      animal.StopMoving();
    }

    public AnimalState Execute(Animal animal)
    {
      return AnimalState.Dead; 
    }
    
  }
}

