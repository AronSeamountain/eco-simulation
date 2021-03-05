using Core;
using UnityEngine;

namespace Animal.AnimalStates
{
  public sealed class IdleState : IState<AnimalState>
  {
    /// <summary>
    ///   The time to stand still in seconds.
    /// </summary>
    private const float MaxIdle = 4f;

    private readonly AbstractAnimal _animal;

    private float _idleTime;

    /// <summary>
    ///   The time the animal has stood still.
    /// </summary>
    private float _timeIdled;

    public IdleState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Idle;
    }

    public void Enter()
    {
      _animal.StopMoving();
      _timeIdled = 0;
      UpdateIdleTime();
    }

    public AnimalState Execute()
    {
      if (_animal.Dead) return AnimalState.Dead;
      if (_animal.ShouldBirth) return AnimalState.Birth;
      if (_animal.enemyToFleeFrom) return AnimalState.Flee;

      var haveIdledSufficiently = _timeIdled >= _idleTime;
      if (haveIdledSufficiently)
        return AnimalState.Wander;

      _timeIdled += Time.deltaTime;
      return AnimalState.Idle;
    }


    public void Exit()
    {
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