using Core;

namespace Animal.AnimalStates
{
  public sealed class BirthState : IState<AnimalState>
  {
    private readonly AbstractAnimal _self;
    private  AbstractAnimal _father;

    public BirthState(AbstractAnimal self)
    {
      _self = self;
      _father = self.FatherToChildren;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Birth;
    }

    public void Enter()
    {
      
      _father = _self.FatherToChildren;
    }

    public AnimalState Execute()
    {
      _self.SpawnChild(_father);
      return AnimalState.Wander;
    }

    public void Exit()
    {
    }
  }
}