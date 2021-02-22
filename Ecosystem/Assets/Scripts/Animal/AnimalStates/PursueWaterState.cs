using Core;
using Foods;
using Utils;

namespace Animal.AnimalStates
{
  public sealed class PursueWaterState : INewState<AnimalState>
  {
    private readonly AbstractAnimal _animal;
    private Water _waterTarget;

    public PursueWaterState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.PursueWater;
    }

    public void Enter()
    {
    }

    public AnimalState Execute()
    {
      if (!_animal.IsAlive) return AnimalState.Dead;
      if (_animal.ShouldBirth) return AnimalState.Birth;
      if (!_animal.IsThirsty) return AnimalState.Wander;
      if (!_animal.KnowsWaterLocation) return AnimalState.Wander;

      _waterTarget = _animal.ClosestKnownWater;
      if (_waterTarget == null) return AnimalState.Wander;

      var reachesWater = Vector3Util.InRange(_animal.gameObject, _waterTarget.gameObject, _animal.Reach);
      if (reachesWater)
      {
        _animal.Drink(_waterTarget);
        return AnimalState.Wander;
      }

      var position = _waterTarget.transform.position;
      _animal.GoTo(position);

      return AnimalState.PursueWater;
    }

    public void Exit()
    {
    }
  }
}