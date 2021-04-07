using Core;
using Foods;
using UnityEngine;
using Utils;

namespace Animal.AnimalStates
{
  public sealed class PursueWaterState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;
    private Water _waterTarget;

    public PursueWaterState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.PursueWater;
    }

    public void Enter()
    {
      _animal.IsRunning = true;
      _animal.SetSpeed();
    }

    public AnimalState Execute()
    {
      if (!_animal.Alive) return AnimalState.Dead;
      if (_animal.ShouldBirth) return AnimalState.Birth;
      if (!_animal.IsThirsty) return AnimalState.Wander;
      if (_animal.EnemyToFleeFrom.Exists())
      {
        if (_animal.KnowsWaterLocation) _animal.KnowsWaterLocation = false;

        return AnimalState.Flee;
      }

      if (!_animal.IsThirsty && _animal.IsHungry && !_animal.KnowsFoodLocation) return AnimalState.SearchWorld;
      if (!_animal.KnowsWaterLocation) return AnimalState.Wander;


      _waterTarget = _animal.ClosestKnownWater;
      if (!_waterTarget) return AnimalState.Wander;

      var position = _animal.transform.position;
      var closestPoint = _waterTarget.GetComponent<Collider>().ClosestPoint(position);
      if (Vector3.Distance(position, closestPoint) < _animal.Reach) return AnimalState.Drink;

      _animal.GoTo(closestPoint);

      return AnimalState.PursueWater;
    }

    public void Exit()
    {
    }
  }
}