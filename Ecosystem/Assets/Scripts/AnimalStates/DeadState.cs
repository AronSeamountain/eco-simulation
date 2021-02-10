using UnityEngine;

namespace AnimalStates
{
  public sealed class DeadState : IState
  {
    private readonly int _rotationSpeed = 10;

    public AnimalState GetStateEnum()
    {
      return AnimalState.Dead;
    }

    public void Enter(Animal animal)
    {
    }

    public AnimalState Execute(Animal animal)
    {
      if (animal._health > 0) return AnimalState.Wander;
      FallOver(animal);
      return AnimalState.Dead;
    }

    public void Exit(Animal animal)
    {
      //animal.StopMoving();
    }

    /// <summary>
    ///   The animal should fall over when it's dead.
    /// </summary>
    private void FallOver(Animal animal)
    {
      
      if (animal._health <= 0)
      {
        animal.StopMoving();
        animal.transform.rotation = Quaternion.Euler(0, 0, 90);
        animal.transform.position = new Vector3(animal.transform.position.x, 0, animal.transform.position.z);
      }
    }
  }
}