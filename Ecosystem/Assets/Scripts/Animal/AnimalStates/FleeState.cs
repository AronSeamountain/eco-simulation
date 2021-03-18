using Core;

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
    }

    public AnimalState Execute()
    {
      if (_animal.Dead) return AnimalState.Dead;
      if (_animal.SafeDistanceFromEnemy()) return AnimalState.Wander;
      if (_animal.EnemyToFleeFrom) _animal.Flee();
      return AnimalState.Flee;
    }

    public void Exit()
    {
      _animal.StopFleeing();
    }
  }
}