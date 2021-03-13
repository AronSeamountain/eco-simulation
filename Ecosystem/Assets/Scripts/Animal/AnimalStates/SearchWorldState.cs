using Core;
using UnityEngine;
using Utils;

namespace Animal.AnimalStates
{
  /// <summary>
  /// Gets invoked when the animal cannot find food or water
  /// and needs to traverse a larger area to be able to find it.
  /// </summary>
  public sealed class SearchWorldState : IState<AnimalState>
  {
    private float closeToDest = 2.0f;
    private readonly AbstractAnimal _animal;
    private Vector3 _destination;

    public SearchWorldState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.SearchWorld;
    }

    public void Enter()
    { //spped is a bit faster than wander, since it nneds to find food or water fast
      _animal.SetSpeed(3);
      GoToFarAwayPoint();
    }

    public AnimalState Execute()
    {
      //if animal has eaten, but can't find water, it should go into this state
      // similarly, if it has drunk enough water but cannot find food, it should go into this state.
      if (!_animal.Alive) return AnimalState.Dead;
      if (_animal.ShouldBirth) return AnimalState.Birth;
      if (_animal.EnemyToFleeFrom) return AnimalState.Flee;
      if (_animal.IsThirsty && _animal.KnowsWaterLocation) return AnimalState.PursueWater;
      if (_animal.IsHerbivore && _animal.KnowsFoodLocation && _animal.IsHungry) return AnimalState.PursueFood;
      if (_animal is Carnivore carnivore && carnivore.IsHungry)
      {
        var target = carnivore.Target;
        if (target && carnivore.ShouldHunt(target)) return AnimalState.Hunt;
      }
      
      if (Vector3Util.InRange(_animal.transform.position, _destination, closeToDest) &&
          !_animal.IsHungry && !_animal.IsThirsty)
        return AnimalState.Wander;
      //Get new point if animal is stuck on some edge
      if (AnimalIsStuck())
      {
        GoToFarAwayPoint();
      }
      
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
    /// <summary>
    /// Check if animal is moving forward or not
    /// </summary>
    /// <returns></returns>
    private bool AnimalIsStuck()
    {
      var oldPoint = _animal.transform.position - new Vector3(0, 0, 0.2f);
      return !(Vector3.Distance(_animal.transform.position, oldPoint) >= 0.2f);
    }
  }
}