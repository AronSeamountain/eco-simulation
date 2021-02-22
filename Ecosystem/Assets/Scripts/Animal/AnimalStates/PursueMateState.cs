using Core;
using UnityEngine;
using Utils;

namespace AnimalStates
{
  public class PursueMateState : INewState<AnimalState>
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
      if (_animal.IsHungry && _animal.KnowsFoodLocation) return AnimalState.PursueFood;
      if (_animal.IsThirsty && _animal.KnowsWaterLocation) return AnimalState.PursueWater;
      

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
        mateTarget.Mate(_animal);
        _animal.ClearMateTarget();
        return AnimalState.Wander;
      }
      else
      {
        _animal.GoTo(mateTarget.transform.position);
      }

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