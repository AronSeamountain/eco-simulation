using Core;

namespace Animal.AnimalStates
{
  public sealed class HuntState : IState<AnimalState>
  {
    private readonly CarnivoreScript _carnivore;
    private HerbivoreScript _target;

    public HuntState(CarnivoreScript carnivore)
    {
      _carnivore = carnivore;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Hunt;
    }

    public void Enter()
    {
      _target = _carnivore.Target;
    }

    public AnimalState Execute()
    {
      if (_target && !_carnivore.IsHungry && _carnivore.IsThirsty) return AnimalState.Wander;
      if (!_target) return AnimalState.Wander;
      if (!_carnivore.ShouldHunt(_target)) return AnimalState.Wander;
      if (!_target.IsAlive) return AnimalState.Wander;
      _carnivore.GoTo(_target.transform.position);
      _carnivore.TakeABiteFromHerbivore(_target);
      return AnimalState.Hunt;
    }

    public void Exit()
    {
      _target = null;
    }
  }
}