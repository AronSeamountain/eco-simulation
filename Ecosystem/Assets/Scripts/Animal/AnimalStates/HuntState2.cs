using System.Collections.Generic;
using System.Linq;
using Animal;
using Foods;
using UnityEngine;
using Utils;
using Animal;

namespace AnimalStates
{
  public sealed class HuntState2 : INewState<AnimalState>
  {
    private CarnivoreScript _carnivore;
    private HerbivoreScript _target;

    public HuntState2(CarnivoreScript carnivore)
    {
      _carnivore = carnivore;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Hunt;
    }

    public void Enter()
    {
      var colliders = Physics.OverlapSphere(_carnivore.transform.position, 50);
      foreach (var collider in colliders)
        if (collider.GetComponent<HerbivoreScript>() is HerbivoreScript f)
        {
          _target = f;
        }
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