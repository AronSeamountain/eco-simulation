using Core;
using UnityEngine;
using Utils;

namespace Animal.AnimalStates
{
  public class PursueMateState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;
    private Vector3 _mateTargetPos;

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
      _animal.IsRunning = true;
      _animal.SetSpeed();
    }

    public AnimalState Execute()
    {
      if (_animal.Dead) return AnimalState.Dead;
      if (_animal.ShouldBirth) return AnimalState.Birth;
      if (_animal.EnemyToFleeFrom.Exists()) return AnimalState.Flee;
      if (_animal.IsThirsty && _animal.KnowsWaterLocation) return AnimalState.PursueWater;
      if (_animal.IsHerbivore && _animal.KnowsFoodLocation && _animal.IsHungry) return AnimalState.PursueFood;
      if (_animal is Carnivore carnivore) // TODO: no no :-)
      {
        var target = carnivore.Target;
        if (target && carnivore.ShouldStartHunt(target)) return AnimalState.Hunt;
      }
      
      var mateTarget = _animal.GetMateTarget();
      if (mateTarget.DoesNotExist()) return AnimalState.Wander;
      if (!mateTarget.Fertile || !mateTarget.IsSatisfied || !_animal.Fertile)
      {
        _animal.ClearMateTarget();
        return AnimalState.Wander;
      }

      var position = _animal.transform.position;
      var closestPoint = mateTarget.animalCollider.ClosestPointOnBounds(position);
      
      var reachesMate = Vector3.Distance(position, closestPoint) < _animal.Reach;
      if (reachesMate)
      {

        AbstractAnimal female;
        AbstractAnimal male;

        if (_animal.Gender == Gender.Female)
        {
          female = _animal;
          male = mateTarget;
        }
        else
        {
          female = mateTarget;
          male = _animal;
        }
        male.StopMoving();
        female.StopMoving();
        // visual cue
        male.EmitMatingCue();
        female.EmitMatingCue();
        
        female.Mate(male);
        _animal.ClearMateTarget();
        male.Children++; // TODO: Small possibility that female dies before birthing
        return AnimalState.Wander;
      }

      if (Vector3Util.InRange(_mateTargetPos, closestPoint, _animal.ChangeTargetThreshold))
        return AnimalState.PursueMate;;
      
      _mateTargetPos = closestPoint;
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