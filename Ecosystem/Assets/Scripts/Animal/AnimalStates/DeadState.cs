﻿using Core;
using Pools;
using UnityEngine;

namespace Animal.AnimalStates
{
  public sealed class DeadState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;

    public DeadState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Dead;
    }

    public void Enter()
    {
      _animal.StopMoving();

      _animal.DiedListeners?.Invoke(_animal);
      AnimalPool.SharedInstance.Pool(_animal);
    }

    public AnimalState Execute()
    {
      _animal.Decay();
      return AnimalState.Dead;
    }

    public void Exit()
    {
    }
  }
}