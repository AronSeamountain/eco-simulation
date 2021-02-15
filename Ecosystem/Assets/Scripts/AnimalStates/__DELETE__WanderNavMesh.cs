using Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AnimalStates
{
  public sealed class __DELETE__WanderNavMesh : IState<Animal, AnimalState>
  {
    public AnimalState GetStateEnum()
    {
      return AnimalState.Wander;
    }

    public void Enter(Animal animal)
    {
    }

    public AnimalState Execute(Animal animal)
    {
      var cam = Camera.main;
      var mousePos = Mouse.current.position.ReadValue();
      var ray = cam.ScreenPointToRay(mousePos);

      if (Physics.Raycast(ray, out var hit))
      {
        animal.SetDestination(hit.point);
      }
      
      return AnimalState.Wander;
    }

    public void Exit(Animal animal)
    {
    }
  }
}