using Core;
using UnityEngine;

namespace Animal.AnimalStates
{
  public class IdleState : IState<AnimalState>
  {
    private const float MaxIdle = 4f;
    private float _timeIdled;
    private float _idleTime;
    private readonly AbstractAnimal _animal;
    
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
      if (!_animal.IsAlive) return AnimalState.Dead;
      if (_animal.ShouldBirth) return AnimalState.Birth;
      var haveIdledSufficiently = _timeIdled >= _idleTime;

      if (haveIdledSufficiently)
      {
        return AnimalState.Wander;
      }
      
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