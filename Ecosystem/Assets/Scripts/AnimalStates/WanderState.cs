using Core;
using UnityEngine;
using UnityEngine.AI;
using Utils;

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
      //if (!animal.IsAlive) return AnimalState.Dead;
      //if (animal.KnowsWaterLocation && animal.IsThirsty) return AnimalState.PursueWater;
      //if (animal.KnowsFoodLocation && animal.IsHungry) return AnimalState.PursueFood;

      if (Vector3Util.InRange(animal.transform.position, _destination, 1f))
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
        {
          Debug.Log("idled: " + _timeIdled);
          _timeIdled += Time.deltaTime;
        }
      }

      return AnimalState.Wander;
    }

    /// <summary>
    ///   Sets the animals target position to a close points.
    /// </summary>
    /// <param name="animal">The animal to move.</param>
    private void GoToClosePoint(Animal animal)
    {
      var point = GetRandomClosePoint(animal.transform.position);
      _destination = point;
      animal.GoTo(_destination);
    }

    /// <summary>
    ///   Gets a new point close to the given origin vector. Will have the same y level.
    /// </summary>
    /// <param name="origin">The point to get a point close to.</param>
    /// <returns>A new point close to the given origin vector.</returns>
    private Vector3 GetRandomClosePoint(Vector3 origin)
    {
      const int radius = 10;

      var randomDirection = origin + Random.insideUnitSphere * radius;
      var finalPosition = Vector3.zero;

      if (NavMesh.SamplePosition(randomDirection, out var hit, radius, 1))
        finalPosition = hit.position;

      // TODO: I believe there is a SLIGHT chance that it may not find a point. How to handle this?
      Debug.Assert(finalPosition != Vector3.zero);
      return finalPosition;
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