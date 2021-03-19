using Core;
using Pools;

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
      _animal.IsRunning = false;
    }

    public AnimalState Execute()
    {
      _animal.Decay();

      if (_animal.NutritionalValue < 0.1)
      {
        _animal.DiedListeners.Invoke(_animal);
        AnimalPool.SharedInstance.Pool(_animal);
      }

      return AnimalState.Dead;
    }

    public void Exit()
    {
    }
  }
}