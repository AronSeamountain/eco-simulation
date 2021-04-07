using Core;
using UnityEngine;
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
      _animal.IsRunning = true;
      _animal.SetSpeed();
    }

    public AnimalState Execute()
    {
      if (_animal.EnemyToFleeFrom.Exists()) return AnimalState.Flee;
      if (_animal.IsThirsty && _animal.KnowsWaterLocation) return AnimalState.PursueWater;
      if (_animal.IsHerbivore && _animal.KnowsFoodLocation && _animal.IsHungry) return AnimalState.PursueFood;
      if (_animal is Carnivore carnivore) // TODO: no no :-)
      {
        var target = carnivore.Target;
        if (target && carnivore.ShouldHunt(target) && carnivore.GetStaminaDelegate().Stamina > 50) return AnimalState.Hunt;
      }

      var mateTarget = _animal.GetMateTarget();

      if (_animal.Dead) return AnimalState.Dead;
      if (_animal.ShouldBirth) return AnimalState.Birth;
      if (mateTarget.DoesNotExist()) return AnimalState.Wander;
      if (!mateTarget.Fertile)
      {
        _animal.ClearMateTarget();
        return AnimalState.Wander;
      }
      
      var position = _animal.transform.position;
      var closestPoint = mateTarget.animalCollider.ClosestPointOnBounds(position);

      var reachesMate = Vector3.Distance(position, closestPoint) < _animal.Reach;
      if (reachesMate)
      {
        _animal.StopMoving();
        mateTarget.StopMoving();
        // visual cue
        _animal.EmitMatingCue();
        mateTarget.EmitMatingCue();
        
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