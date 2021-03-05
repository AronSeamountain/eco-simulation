using Core;
using UnityEngine;
using Utils;

namespace Animal.AnimalStates
{
  /// <summary>
  ///   A state for an animal which walks randomly.
  /// </summary>
  public sealed class WanderState : IState<AnimalState>
  {
    private const float MarginToReachDestination = 2.5f;
    private readonly AbstractAnimal _animal;

    private Vector3 _destination;

    public WanderState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Wander;
    }

    public void Enter()
    {
      GoToClosePoint();
    }

    public void Exit()
    {
    }

    public AnimalState Execute()
    {
      var isSatisfied = !_animal.IsHungry && !_animal.IsThirsty;

      if (!_animal.IsAlive) return AnimalState.Dead;
      if (_animal.ShouldBirth) return AnimalState.Birth;
      if (_animal.enemyToFleeFrom) return AnimalState.Flee;
      if (_animal.IsThirsty && _animal.KnowsWaterLocation) return AnimalState.PursueWater;
      if (isSatisfied && _animal.GetMateTarget() && _animal.Gender == Gender.Male) return AnimalState.PursueMate;
      if (_animal.IsHerbivore && _animal.KnowsFoodLocation && _animal.IsHungry) return AnimalState.PursueFood;
      if (_animal is Carnivore carnivore) // TODO: no no :-)
      {
        var target = carnivore.Target;
        if (target && carnivore.ShouldHunt(target)) return AnimalState.Hunt;
      }

      if (Vector3Util.InRange(_animal.transform.position, _destination, MarginToReachDestination))
        return AnimalState.Idle;

      return AnimalState.Wander;
    }

    /// <summary>
    ///   Sets the animals target position to a close points.
    /// </summary>
    /// <param name="animal">The animal to move.</param>
    private void GoToClosePoint()
    {
      var point = NavMeshUtil.GetRandomClosePoint(_animal.transform.position);
      _destination = point;
      _animal.GoTo(_destination);
    }
  }
}