using AnimalStates;
using UnityEngine;
using Utils;

public sealed class PursueWaterState : IState
{
  private Water _waterTarget;

  public AnimalState GetStateEnum()
  {
    return AnimalState.PursueWater;
  }

  public void Enter(Animal animal)
  {
  }

  public AnimalState Execute(Animal animal)
  {
    animal.HydrationSaturationTicker();
    if (!animal.IsThirsty) return AnimalState.Wander;
    if (!animal.KnowsWaterLocation) return AnimalState.Wander;
    
    //Enter dead state
    if (animal.GetHealth() <= 0) return AnimalState.Dead;

    _waterTarget = animal.ClosestKnownWater;
    if (_waterTarget == null) return AnimalState.Wander;


    var reachesWater = Vector3Util.InRange(animal.gameObject, _waterTarget.gameObject, 2);
    if (reachesWater)
    {
      animal.Drink(_waterTarget);
      return AnimalState.Wander;
    }

    var position = _waterTarget.transform.position;
    animal.GoTo(position);

    return AnimalState.PursueWater;
  }

  public void Exit(Animal animal)
  {
    Debug.Log("EXIT PURSUE WATER");
  }
}