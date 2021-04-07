using Core;
using JetBrains.Annotations;
using UnityEngine;
using Utils;

namespace Animal.AnimalStates
{
  public sealed class FleeState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;

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
      _animal.IsRunning = true;
      _animal.SetSpeed();
      _animal.EmitFleeCue();
    }

    public AnimalState Execute()
    {
      if (_animal.Dead) return AnimalState.Dead;
      if (_animal.SafeDistanceFromEnemy()) return AnimalState.Wander;
      if (_animal.EnemyToFleeFrom.Dead || _animal.EnemyToFleeFrom.DoesNotExist()) return AnimalState.Wander;

      if (_animal.EnemyToFleeFrom.Exists() && !_animal.IsMoving)
      {
        var t = _animal.transform;
        var point = NavMeshUtil.GetRandomClosePoint(t.position + t.forward);
        Debug.Log("bout to flee");
        _animal.GoTo(point);
      }

      return AnimalState.Flee;
    }

    public void Exit()
    {
      _animal.StopFleeing();
    }
  }
}