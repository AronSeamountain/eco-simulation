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
      animal.StopMoving();
    }

    public AnimalState Execute(Animal animal)
    {
      FallOver(animal);
      return AnimalState.Dead;
    }

    public void Exit(Animal animal)
    {
    }

    /// <summary>
    ///   The animal should fall over when it's dead.
    /// </summary>
    private void FallOver(Animal animal)
    {
      animal.transform.rotation = Quaternion.Euler(0, 0, 90);
      animal.transform.position = new Vector3(animal.transform.position.x, 0, animal.transform.position.z);
    }
  }
}