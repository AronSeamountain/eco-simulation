using System;
using Core;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Random = UnityEngine.Random;

namespace AnimalStates
{
  /// <summary>
  ///   A state for an animal which walks randomly.
  /// </summary>
  public sealed class WanderState : IState<Animal, AnimalState>
  {
    /// <summary>
    ///   The time to stand still in seconds.
    /// </summary>
    private const float MaxIdle = 4f;

    private Vector3 _destination;
    private const float MarginToReachDestination = 2.5f;

    /// <summary>
    ///   The time the animal should stand still.
    /// </summary>
    private float _idleTime;

    /// <summary>
    ///   The time the animal has stood still.
    /// </summary>
    private float _timeIdled;

    public AnimalState GetStateEnum()
    {
      return AnimalState.Wander;
    }

    public void Enter(Animal animal)
    {
      GoToClosePoint(animal);
      UpdateIdleTime();
    }

    public void Exit(Animal animal)
    {
      animal.StopMoving();
    }

    public AnimalState Execute(Animal animal)
    {
      if (!animal.IsAlive) return AnimalState.Dead;
      if (animal.KnowsWaterLocation && animal.IsThirsty) return AnimalState.PursueWater;
      if (animal.KnowsFoodLocation && animal.IsHungry) return AnimalState.PursueFood;

      if (Vector3Util.InRange(animal.transform.position, _destination, MarginToReachDestination))
        animal.StopMoving();

      if (!animal.IsMoving)
      {
        var haveIdledSufficiently = _timeIdled >= _idleTime;

        if (haveIdledSufficiently)
        {
          GoToClosePoint(animal);
          UpdateIdleTime();
          _timeIdled = 0;
        }
        else
          _timeIdled += Time.deltaTime;
      }

      return AnimalState.Wander;
    }

    /// <summary>
    ///   Sets the animals target position to a close points.
    /// </summary>
    /// <param name="animal">The animal to move.</param>
    private void GoToClosePoint(Animal animal)
    {
      var point = NavMeshUtil.GetRandomClosePoint(animal.transform.position);
      _destination = point;
      animal.GoTo(_destination);
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