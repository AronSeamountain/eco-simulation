using Core;
using Utils;

namespace Animal.AnimalStates
{
  public class PursueMateState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;

    public PursueMateState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.PursueMate;
    }

    public void Enter()
    {
    }

    public AnimalState Execute()
    {
      if (!_animal.IsAlive) return AnimalState.Dead;
      if (_animal.ShouldBirth) return AnimalState.Birth;
      if (_animal.enemyToFleeFrom) return AnimalState.Flee;

      var mateTarget = _animal.GetMateTarget();
      if (mateTarget == null) return AnimalState.Wander;
      if (!mateTarget.Fertile)
      {
        _animal.ClearMateTarget();
        return AnimalState.Wander;
      }

      var reachesMate = Vector3Util.InRange(_animal.gameObject, mateTarget.gameObject, 2);
      if (reachesMate)
      {
        _animal.StopMoving();
        mateTarget.StopMoving();
        mateTarget.Mate(_animal);
        _animal.ClearMateTarget();
        _animal.Children++; // TODO: Small possibility that female dies before birthing
        return AnimalState.Wander;
      }

      _animal.GoTo(mateTarget.transform.position);

      return AnimalState.PursueMate;
    }

    public void Exit()
    {
    }

    //Todo add some reasonable conditions
    private bool CanMate()
    {
      return true;
    }
  }
}