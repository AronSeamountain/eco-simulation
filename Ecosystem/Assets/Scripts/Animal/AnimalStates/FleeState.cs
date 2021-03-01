using Core;
using Utils;


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
      //Add more checks.
      return AnimalState.Flee;
    }

    public void Exit()
    {
    }
    
  }
}
