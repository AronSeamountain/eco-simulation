using Core;
using UnityEngine;
using Utils;

namespace Animal.AnimalStates
{
  /// <summary>
  ///   Gets invoked when the animal cannot find food or water
  ///   and needs to traverse a larger area to be able to find it.
  /// </summary>
  public sealed class SearchWorldState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;
    private readonly float closeToDest = 2.0f;
    private Vector3 _destination;
    private float _distance;

    public SearchWorldState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.SearchWorld;
    }

    public void Enter()
    {
      //speed is a bit faster than wander, since it needs to find food or water fast
      _animal.IsRunning = true;
      _animal.SetSpeed();
      MonoBehaviour walkablePoint =  NavMeshUtil.getRandomWalkablePoint();
      _animal.GoTo(walkablePoint.gameObject.transform.position);
    }

    public AnimalState Execute()
    {
      //if animal has eaten, but can't find water, it should go into this state
      // similarly, if it has drunk enough water but cannot find food, it should go into this state.
      if (!_animal.Alive) return AnimalState.Dead;
      if (_animal.ShouldBirth) return AnimalState.Birth;
      if (_animal.EnemyToFleeFrom.Exists()) return AnimalState.Flee;
      if (_animal.IsThirsty && _animal.KnowsWaterLocation) return AnimalState.PursueWater;
      if (_animal.IsHerbivore && _animal.KnowsFoodLocation && _animal.IsHungry) return AnimalState.PursueFood;
      if (_animal is Carnivore carnivore && carnivore.IsHungry)
      {
        var target = carnivore.Target;
        if (target && carnivore.ShouldHunt(target)) return AnimalState.Hunt;
      }

      
      /*if (Vector3Util.InRange(_animal.transform.position, _destination, closeToDest) &&
          !_animal.IsHungry && !_animal.IsThirsty)
        return AnimalState.Wander;

      //If animal is trying to run outside the navmesh, find a new point to go to.
      if (!IsMovingForward()) GoToFarAwayPoint();*/

      return AnimalState.SearchWorld;
    }

    public void Exit()
    {
    }

    private void GoToFarAwayPoint()
    {
      var point = NavMeshUtil.GetRandomPointFarAway(_animal.transform.position);
      _destination = point;
      _animal.GoTo(_destination);
    }

    //checks if distance to destination shrinks. If not, the animal should find new position.
    private bool IsMovingForward()
    {
      var distTemp = Vector3.Distance(_destination, _animal.transform.position);
      if (distTemp < _distance)
      {
        _distance = distTemp;
        return true;
      }

      if (distTemp >= _distance)
      {
        _distance = distTemp;
        return false;
      }

      return false;
    }
  }
}