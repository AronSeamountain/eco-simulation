﻿using System.Collections;
using System.Collections.Generic;
using AnimalStates;
using UnityEngine;

public class PursueWaterState : IState
{
  private Water _waterTarget;

  public AnimalState GetStateEnum()
  {
    return AnimalState.PursueWater;
  }

  public void Enter(Animal animal)
  {
  }

  public AnimalState Execute(Animal animal)
  {
    if (!animal.KnowsWaterLocation) return AnimalState.Wander;

    _waterTarget = animal.ClosestKnownWater;
    if (_waterTarget == null) return AnimalState.Wander;

    var position = _waterTarget.transform.position;

    var reachesWater = (animal.transform.position - position).magnitude < 2;
    if (reachesWater)
    {
      animal.Drink(_waterTarget);
    }

    animal.GoTo(position);


    return AnimalState.PursueWater;
  }


  public void Exit(Animal animal)
  {
    throw new System.NotImplementedException();
  }
}