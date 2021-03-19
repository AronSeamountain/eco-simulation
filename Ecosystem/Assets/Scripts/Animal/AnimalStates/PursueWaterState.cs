using System;
using Core;
using Foods;
using UnityEngine;
using Utils;

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
      _animal.IsRunning = true;
      _animal.SetSpeed();
    }

    public AnimalState Execute()
    {
      if (!_animal.Alive) return AnimalState.Dead;
      if (_animal.ShouldBirth) return AnimalState.Birth;
      if (!_animal.IsThirsty) return AnimalState.Wander;
      if (_animal.EnemyToFleeFrom) return AnimalState.Flee;
      if (!_animal.IsThirsty && _animal.IsHungry && !_animal.KnowsFoodLocation) return AnimalState.SearchWorld;
      if (!_animal.KnowsWaterLocation) return AnimalState.Wander;
      

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
    /// Shoots a ray at the water source and checks that the length of the ray is less than 3 (no real reason for 3, it works)
    /// </summary>
    /// <returns>true if the water is in range, false otherwise</returns>
    private bool ReachedWater()
    {
      var position = _animal.transform.position;
      var layerNames = new string[1];
      layerNames[0] = "Water";
      
      RaycastHit hit = RayCastUtil.RayCastLayer(position, _waterTarget.transform.position - position, layerNames);
      if (!hit.collider) return false; 
      if (hit.collider.gameObject.GetComponent<Water>() == _waterTarget)
      {
        return hit.distance < 3;
      }
      return false;
    }
  }
}