﻿using Core;
using UnityEngine;
using Utils;

namespace Animal.AnimalStates
{
  public sealed class HuntState : IState<AnimalState>
  {
    private readonly Carnivore _carnivore;
    private Herbivore _target;

    public HuntState(Carnivore carnivore)
    {
      _carnivore = carnivore;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Hunt;
    }

    public void Enter()
    {
      _carnivore.IsRunning = true;
      _carnivore.SetSpeed();
      _carnivore.IsHunting = true;
      _target = _carnivore.Target;
    }

    public AnimalState Execute()
    {
      if (!_carnivore.Alive) return AnimalState.Dead;
      if (_carnivore.IsThirsty && !_carnivore.KnowsWaterLocation && !_carnivore.IsHungry)
        return AnimalState.SearchWorld;
      if (!_carnivore.ShouldHunt(_target))
        return AnimalState.Wander;

      if (_target.DoesNotExist())
      {
        _carnivore.Target = null;
        return AnimalState.Wander;
      }

      _carnivore.GoTo(_target.transform.position);

      if (Vector3Util.InRange(_carnivore, _target, Carnivore.EatingRange))
      {
        if (!_target.Alive)
        {
          _carnivore.FoodAboutTooEat = _target;
          return AnimalState.Eat;
        }

        _carnivore.SetMouthColor(Color.red);
        _carnivore.AttackTarget(_target);
      }

      return AnimalState.Hunt;
    }

    public void Exit()
    {
      _target = null;
      _carnivore.IsHunting = false;
    }
  }
}