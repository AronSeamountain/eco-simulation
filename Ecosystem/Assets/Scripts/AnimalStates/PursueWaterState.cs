using Core;
using UnityEngine;
using Utils;

namespace AnimalStates
{
  public sealed class PursueWaterState : IState<Animal, AnimalState>
  {
    private Water _waterTarget;

    public AnimalState GetStateEnum()
    {
      return AnimalState.PursueWater;
    }

    public void Enter(Animal animal)
    {
      animal.DisplayState();
    }

    public AnimalState Execute(Animal animal)
    {
      if (!animal.IsThirsty) return AnimalState.Wander;
      if (!animal.KnowsWaterLocation) return AnimalState.Wander;

      _waterTarget = animal.ClosestKnownWater;
      if (_waterTarget == null) return AnimalState.Wander;

      if (ReachedWater(animal))
      {
        animal.Drink(_waterTarget);
        return AnimalState.Wander;
      }

      var position = _waterTarget.transform.position;
      animal.GoTo(position);

      return AnimalState.PursueWater;
    }

    /// <summary>
    /// Shoots a ray at the water source and checks that the length of the ray is less than 2 (no real reason for 2, it works)
    /// </summary>
    /// <param name="animal">The animal</param>
    /// <returns>true if the water is in range, false otherwise</returns>
    private bool ReachedWater(Animal animal)
    {
      var position = animal.transform.position;
      Ray ray = new Ray(position, _waterTarget.transform.position - position);
      Physics.Raycast(ray, out var hit);
      if (hit.collider.gameObject.GetComponent<Water>() == _waterTarget)
      {
        return hit.distance < 2;
      }

      return false;
    }

    public void Exit(Animal animal)
    {
    }
  }
}