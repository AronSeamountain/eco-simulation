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
    }

    public AnimalState Execute()
    {
      if (!_animal.IsAlive) return AnimalState.Dead;
      if (_animal.SafeDistanceFromEnemy(_animal.enemyToFleeFrom)) return AnimalState.Wander;
      if (_animal.enemyToFleeFrom != null) _animal.Flee();
      return AnimalState.Flee;
    }

    public void Exit()
    {
      _animal.StopFleeing();
    }
  }
}