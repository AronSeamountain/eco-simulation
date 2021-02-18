﻿using Animal;
using UnityEngine;

namespace AnimalStates
{
  public sealed class HuntState : INewState<AnimalState>
  {
    private CarnivoreScript _carnivore;
    private HerbivoreScript _target;

    public HuntState(CarnivoreScript carnivore)
    {
      _carnivore = carnivore;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Hunt;
    }

    public void Enter()
    {
      //_carnivore.OnPreyFound(_target);
      _target = _carnivore.Target;
      return;
      var colliders = Physics.OverlapSphere(_carnivore.transform.position, 50);
      foreach (var collider in colliders)
        if (collider.GetComponent<HerbivoreScript>() is HerbivoreScript f)
          _target = f;
          
    }

    public AnimalState Execute()
    {
      if (!_target) return AnimalState.Wander;
      _carnivore.GoTo(_target.transform.position);
      return AnimalState.Hunt;
    }

    public void Exit()
    {
      _target = null;
    }

  }
}