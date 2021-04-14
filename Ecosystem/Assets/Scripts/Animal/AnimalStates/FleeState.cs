using System;
using Core;
using UnityEngine;
using Utils;

namespace Animal.AnimalStates
{
  public sealed class FleeState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;
    private int _framesFleeingToPos;
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
      _framesFleeingToPos = 20000;
      _animal.IsRunning = true;
      _animal.SetSpeed();
      _animal.EmitFleeCue();
      if (_animal.KnowsWaterLocation || _animal.HasForgottenWater)
      {
        _animal.ForgetWaterLocationForSomeTime();
      }

    }

    public AnimalState Execute()
    {
      if (_animal.Dead) return AnimalState.Dead;
      if (_animal.SafeDistanceFromEnemy()) return AnimalState.Wander;
      if (_animal.EnemyToFleeFrom.Dead || _animal.EnemyToFleeFrom.DoesNotExist()) return AnimalState.Wander;
      if (_animal.EnemyToFleeFrom.Exists() && _framesFleeingToPos > 50)
      {
        _framesFleeingToPos = 1;
        _animal.Flee();
      }
      else
      {
        _framesFleeingToPos++;
      }
      return AnimalState.Flee;
    }

    public void Exit()
    {
      _animal.StopFleeing();
    }
  }
}