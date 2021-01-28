namespace AnimalStates
{
  public interface IState
  {
    void Enter(Animal animal);

    IState Execute(Animal animal);

    void Exit(Animal animal);
  }
}