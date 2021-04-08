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
    private GameObject _destination;
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
      var walkablePoint = _animal.WorldPointFinder.getRandomWalkablePoint(_animal);

      _destination = walkablePoint.gameObject;
      _animal.GoTo(_destination.transform.position);
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
      if (_animal is Carnivore carnivore)
      {
        var target = carnivore.Target;
        if (target && carnivore.ShouldHunt(target) && carnivore.GetStaminaDelegate().Stamina > 50) return AnimalState.Hunt;
      }

      var position = _animal.gameObject.transform.position;
      var closestPoint = _destination.GetComponent<Collider>().ClosestPoint(position);
      if (Vector3.Distance(position, closestPoint) < _animal.Reach * 2)
        return AnimalState.Wander; // needs to change state so that it finds a new walkablePoint


      return AnimalState.SearchWorld;
    }

    public void Exit()
    {
    }
  }
}