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

      //var reachedWater(animal);// = Vector3Util.InRange(animal.gameObject, _waterTarget.gameObject, 2);
      if (ReachedWater(animal))
      {
        animal.Drink(_waterTarget);
        return AnimalState.Wander;
      }

      var position = _waterTarget.transform.position;
      animal.GoTo(position);

      return AnimalState.PursueWater;
    }

    private bool ReachedWater(Animal animal)
    {
      var distance = 2;
      return Vector3Util.InRange(animal.gameObject, _waterTarget.gameObject, distance);
    }

    public void Exit(Animal animal)
    {
    }
  }
}