using Animal;
using UnityEngine;

namespace AnimalStates
{
  /// <summary>
  ///   A state for an animal which walks randomly.
  /// </summary>
  public sealed class WanderState : INewState<AnimalState>
  {
    /// <summary>
    ///   The time to stand still in seconds.
    /// </summary>
    private const float MaxIdle = 4f;

    private readonly AbstractAnimal _animal;

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
      GoToClosePoint(_animal);
      UpdateIdleTime();
    }

    public void Exit()
    {
      _animal.StopMoving();
    }

    public AnimalState Execute()
    {
      // Enter pursue water state
      if (_animal.KnowsWaterLocation && _animal.IsThirsty)
        return AnimalState.PursueWater;

      // Enter pursue food state
      if (_animal.KnowsFoodLocation && _animal.IsHungry)
        return AnimalState.PursueFood;

      //Enter dead state
      if (!_animal.IsAlive)
        return AnimalState.Dead;

      var shouldMoveToNewPos = !_animal.IsMoving && _timeIdled >= _idleTime;
      if (shouldMoveToNewPos)
      {
        GoToClosePoint(_animal);
        UpdateIdleTime();
        _timeIdled = 0;
      }
      else
      {
        _timeIdled += Time.deltaTime;
      }

      return AnimalState.Wander;
    }

    /// <summary>
    ///   Sets the animals target position to a close points.
    /// </summary>
    /// <param name="animal">The animal to move.</param>
    private void GoToClosePoint(AbstractAnimal animal)
    {
      var point = GetRandomClosePoint(animal.transform.position);
      animal.GoTo(point);
    }

    /// <summary>
    ///   Gets a new point close to the given origin vector. Will have the same y level.
    /// </summary>
    /// <param name="origin">The point to get a point close to.</param>
    /// <returns>A new point close to the given origin vector.</returns>
    private Vector3 GetRandomClosePoint(Vector3 origin)
    {
      const int radius = 10;
      return origin + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
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