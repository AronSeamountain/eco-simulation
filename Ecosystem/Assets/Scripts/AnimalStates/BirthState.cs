using Core;
using UnityEngine;

namespace AnimalStates
{
  public sealed class BirthState : IState<Animal, AnimalState>
  {
    public AnimalState GetStateEnum()
    {
      return AnimalState.Birth;
    }

    public void Enter(Animal animal)
    {
      animal.DisplayState();
    }

    public AnimalState Execute(Animal animal)
    {
      animal.SpawnChild();
      return AnimalState.Wander;
    }

    public void Exit(Animal animal)
    {
    }
  }
}