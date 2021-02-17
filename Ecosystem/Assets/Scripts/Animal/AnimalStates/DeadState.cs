using Animal;
using Core;
using UnityEngine;

namespace AnimalStates
{
  public sealed class DeadState : INewState<AnimalState>
  {
    private AbstractAnimal _animal;

    public AnimalState GetStateEnum()
    {
      return AnimalState.Dead;
    }

    public DeadState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public void Enter()
    {
      FallOver(_animal);
    }

    public AnimalState Execute()
    {
      return AnimalState.Dead;
    }

    public void Exit()
    {
    }

    /// <summary>
    ///   Stops the animal from moving and lays it on the floor.
    /// </summary>
    private void FallOver(AbstractAnimal animal)
    {
      animal.StopMoving();
      animal.transform.rotation = Quaternion.Euler(0, 0, 90);
      animal.transform.position = new Vector3(animal.transform.position.x, 0, animal.transform.position.z);
    }
  }
}