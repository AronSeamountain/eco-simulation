﻿using Core;
using UnityEngine;
using Utils;

namespace Animal.AnimalStates
{
  /// <summary>
  ///   A state for an animal which walks randomly.
  /// </summary>
  public sealed class WanderState : IState<AnimalState>
  {
    /// <summary>
    ///   The time to stand still in seconds.
    /// </summary>
    private const float MaxIdle = 4f;

    private const float MarginToReachDestination = 2.5f;
    private readonly AbstractAnimal _animal;

    private Vector3 _destination;

    /// <summary>
    ///   The time the animal should stand still.
    /// </summary>
    private float _idleTime;

    /// <summary>
    ///   The time the animal has stood still.
    /// </summary>
    private float _timeIdled;

    public WanderState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Wander;
    }

    public void Enter()
    {
      GoToClosePoint();
      UpdateIdleTime();
    }

    public void Exit()
    {
      _animal.StopMoving();
    }

    public AnimalState Execute()
    {
      if (!_animal.IsAlive) return AnimalState.Dead;
      if (_animal.ShouldBirth) return AnimalState.Birth;
      if (_animal.KnowsWaterLocation && _animal.IsThirsty) return AnimalState.PursueWater;
      if (_animal.KnowsFoodLocation && _animal.IsHungry) return AnimalState.PursueFood;

      if (_animal.GetMateTarget() != null && _animal.Gender == Gender.Male) return AnimalState.PursueMate;

      if (Vector3Util.InRange(_animal.transform.position, _destination, MarginToReachDestination))
        _animal.StopMoving();

      if (!_animal.IsMoving)
      {
        var haveIdledSufficiently = _timeIdled >= _idleTime;

        if (haveIdledSufficiently)
        {
          GoToClosePoint();
          UpdateIdleTime();
          _timeIdled = 0;
        }
        else
        {
          _timeIdled += Time.deltaTime;
        }
      }

      return AnimalState.Wander;
    }

    /// <summary>
    ///   Sets the animals target position to a close points.
    /// </summary>
    /// <param name="animal">The animal to move.</param>
    private void GoToClosePoint()
    {
      var point = NavMeshUtil.GetRandomClosePoint(_animal.transform.position);
      _destination = point;
      _animal.GoTo(_destination);
    }

    /// <summary>
    ///   Sets the idle time to a value between 0 and the max idle time.
    /// </summary>
    private void UpdateIdleTime()
    {
      _idleTime = Random.Range(0, MaxIdle);
    }
  }
}