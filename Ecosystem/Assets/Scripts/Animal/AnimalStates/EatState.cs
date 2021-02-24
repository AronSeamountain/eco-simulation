using Core;
using Foods;

namespace Animal.AnimalStates
{
  public sealed class EatState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;
    private AbstractFood _food;

    public EatState(AbstractAnimal animal, AbstractFood foodTarget)
    {
      _animal = animal;
      _food = foodTarget;
    }
    
    public AnimalState GetStateEnum()
    {
      return AnimalState.Eat;
    }

    public void Enter()
    {
      //take bite
    }

    public AnimalState Execute()
    {
      //animation and stuff
      
      //if eaten enough return wander

      return AnimalState.Eat;
    }

    public void Exit()
    {
      //stop eating
    }
  }
}