using Core;

namespace Animal.AnimalStates
{
  public sealed class DeadState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;

    public DeadState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Dead;
    }

    public void Enter()
    {
      _animal.StopMoving();
    }

    public AnimalState Execute()
    {
      _animal.Decay();
      return AnimalState.Dead;
    }

    public void Exit()
    {
    }
  }
}