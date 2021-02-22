using Core;

namespace Animal.AnimalStates
{
  public sealed class BirthState : INewState<AnimalState>
  {
    private readonly AbstractAnimal _animal;

    public BirthState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Birth;
    }

    public void Enter()
    {
    }

    public AnimalState Execute()
    {
      _animal.SpawnChild();
      return AnimalState.Wander;
    }

    public void Exit()
    {
    }
  }
}