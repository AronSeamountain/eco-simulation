using Core;
using UnityEngine;
using Utils;

namespace Animal.AnimalStates
{
  public sealed class FleeState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;
    private float _counter;
    private const float TimeBetweenFleePos = 2f;

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

      _counter = 0;
      GoToRandomPos();
    }

    public AnimalState Execute()
    {
      if (_animal.Dead) return AnimalState.Dead;
      if (_animal.SafeDistanceFromEnemy()) return AnimalState.Wander;
      if (_animal.EnemyToFleeFrom.Dead || _animal.EnemyToFleeFrom.DoesNotExist()) return AnimalState.Wander;

      _counter += Time.deltaTime;

      if (_counter >= TimeBetweenFleePos)
      {
        GoToRandomPos();
        _counter = 0;
      }

      return AnimalState.Flee;
    }

    private void GoToRandomPos()
    {
      var walkablePoint = _animal.WorldPointFinder.GetRandomWalkablePoint(_animal);
      _animal.GoTo(walkablePoint.transform.position);
    }

    public void Exit()
    {
      _animal.StopFleeing();
    }
  }
}