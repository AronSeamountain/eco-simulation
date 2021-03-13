using Core;
using Foods;
using UnityEngine;

namespace Animal.AnimalStates
{
  public sealed class PursueWaterState : IState<AnimalState>
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
      _animal.SetSpeed(5);
    }

    public AnimalState Execute()
    {
      if (!_animal.Alive) return AnimalState.Dead;
      if (_animal.ShouldBirth) return AnimalState.Birth;
      if (!_animal.IsThirsty) return AnimalState.Wander;
      if (_animal.EnemyToFleeFrom) return AnimalState.Flee;
      if (!_animal.KnowsWaterLocation) return AnimalState.Wander;
      if (!_animal.IsThirsty && !_animal.KnowsFoodLocation) return AnimalState.SearchWorld;

      _waterTarget = _animal.ClosestKnownWater;
      if (!_waterTarget) return AnimalState.Wander;

      if (ReachedWater()) return AnimalState.Drink;

      var position = _waterTarget.transform.position;
      _animal.GoTo(position);

      return AnimalState.PursueWater;
    }

    public void Exit()
    {
    }

    /// <summary>
    ///   Shoots a ray at the water source and checks that the length of the ray is less than 2 (no real reason for 2, it
    ///   works)
    /// </summary>
    /// <returns>true if the water is in range, false otherwise</returns>
    private bool ReachedWater()
    {
      var position = _animal.transform.position;
      var ray = new Ray(position, _waterTarget.transform.position - position);
      Physics.Raycast(ray, out var hit);
      if (!hit.collider) return false;
      if (hit.collider.gameObject.GetComponent<Water>() == _waterTarget) return hit.distance < 2;

      return false;
    }
  }
}