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
      FallOver(animal);
    }

    public AnimalState Execute(Animal animal)
    {
      return AnimalState.Dead;
    }

    public void Exit(Animal animal)
    {
    }

    /// <summary>
    ///   Stops the animal from moving and lays it on the floor.
    /// </summary>
    private void FallOver(Animal animal)
    {
      animal.StopMoving();
      animal.transform.rotation = Quaternion.Euler(0, 0, 90);
      animal.transform.position = new Vector3(animal.transform.position.x, 0, animal.transform.position.z);
    }
  }
}