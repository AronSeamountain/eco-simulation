using Core;
using Utils;

namespace Animal.AnimalStates
{
  public sealed class HuntState : IState<AnimalState>
  {
    private readonly Carnivore _carnivore;
    private Herbivore _target;

    public HuntState(Carnivore carnivore)
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
      if (!_target || !_target.IsAlive || _carnivore.ShouldHunt(_target))
        return AnimalState.Wander;

      _carnivore.GoTo(_target.transform.position);

      if (Vector3Util.InRange(_carnivore, _target, _carnivore.EatingRange))
        _carnivore.TakeABiteFromHerbivore(_target);

      return AnimalState.Hunt;
    }

    public void Exit()
    {
      _target = null;
    }
  }
}