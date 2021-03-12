using Core;

namespace Animal.AnimalStates
{
  public sealed class SearchWorldState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;

    public SearchWorldState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.SearchWorld;
    }

    public void Enter()
    {
    }

    public AnimalState Execute()
    {
      //if animal has eaten, but can't find water, it should go into this state
      // similarly, if it has drunk enough water but cannot find food, it should go into this state.
      
      return AnimalState.SearchWorld;
    }

    public void Exit()
    {
    }
  }
}