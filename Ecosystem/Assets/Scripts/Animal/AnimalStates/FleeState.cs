using System;
using Core;
using UnityEngine;
using Utils;

namespace Animal.AnimalStates
{
  public sealed class FleeState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;
    private float deltaTimeTracker;
    public FleeState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Flee;
    }

    public void Enter()
    {
      deltaTimeTracker = 1000000; //arbitrarily large value
      _animal.IsRunning = true;
      _animal.SetSpeed();
      _animal.EmitFleeCue();
      if (_animal.KnowsWaterLocation || _animal.HasForgottenWater)
      {
        _animal.ForgetWaterLocationForSomeTime();
        _animal.ForgetFoodLocations();
      }

    }

    public AnimalState Execute()
    {
      if (_animal.Dead) return AnimalState.Dead;
      if (_animal.SafeDistanceFromEnemy()) return AnimalState.Wander;
      if (_animal.EnemyToFleeFrom.Dead || _animal.EnemyToFleeFrom.DoesNotExist()) return AnimalState.Wander;
      deltaTimeTracker = deltaTimeTracker + Time.deltaTime;
      if (_animal.EnemyToFleeFrom.Exists() && deltaTimeTracker > 0.5f)
      {
        deltaTimeTracker = 0;
        _animal.Flee();
      }
      return AnimalState.Flee;
    }

    public void Exit()
    {
      _animal.StopFleeing();
    }
  }
}