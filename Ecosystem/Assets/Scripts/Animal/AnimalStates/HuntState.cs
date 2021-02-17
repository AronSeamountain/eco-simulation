using System.Collections.Generic;
using System.Linq;
using Animal;
using Core;
using UnityEngine;
using Utils;

namespace AnimalStates
{
  /// <summary>
  ///   An animal state that chases the closest food the animal is aware of. Enters wander state when the animal isn't aware
  ///   of any food sources.
  /// </summary>
  public sealed class HuntState : INewState<AnimalState> 
  {
    private bool _knownPreyTargetsChanged;
    private PreyManager.PreyMemory _preyTarget;
    private CarnivoreScript _carnivore;

    public HuntState(CarnivoreScript carnivore)
    {
      _carnivore = carnivore;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Hunt;
    }

    public void Enter()
    {
    }

    public AnimalState Execute()
    {
      if (_carnivore.ShouldBirth) return AnimalState.Birth;
      if (!_carnivore.IsHungry) return AnimalState.Wander;
      if (!_carnivore.IsAlive) return AnimalState.Dead;

      // A new food source has been found. Change the food target to the closest food.
      if (_knownPreyTargetsChanged)
      {
        _knownPreyTargetsChanged = false;
        if (!_carnivore.KnownPrey.Any()) return AnimalState.Wander;

        _preyTarget = GetClosestFood(_carnivore);
        _carnivore.GoTo(_preyTarget.Position);
      }

      // Eat the current food if it can be reached.
      var reachesFood = Vector3Util.InRange(_carnivore.transform.position, _preyTarget.Position, 2);
      if (reachesFood)
      {
        var colliders = Physics.OverlapSphere(_carnivore.transform.position, 2);
        foreach (var collider in colliders)
          if (collider.GetComponent<HerbivoreScript>() is HerbivoreScript prey) 
            if (prey == _preyTarget.Animal)
            {
              _carnivore.StopMoving();
              //_carnivore.Eat(_preyTarget.Animal); 
              break;
            }
        _carnivore.Forget(_preyTarget); 
        _preyTarget = null;
      }

      return AnimalState.Hunt;
    }

    public void Exit()
    {
    }

    /// <summary>
    ///   Returns the closest food the carnivore knows about. Returns null if it knows no foods.
    /// </summary>
    /// <param name="carnivore">The carnivore.</param>
    /// <returns>The closest food.</returns>
    private PreyManager.PreyMemory GetClosestFood(CarnivoreScript carnivore)
    { 
      var preys = carnivore.KnownPrey;
      return preys.OrderBy(prey => Vector3Util.Distance(carnivore.transform.position, prey.Position)).First();
    }

    /// <summary>
    ///   Lets the state know that there is potentially a closer food.
    /// </summary>
    /// <param name="preys">The set of preys the carnivore knows of.</param>
    public void OnKnownPreyLocationsChanged(IEnumerable<PreyManager.PreyMemory> preys)
    {
      _knownPreyTargetsChanged = true;
    }
  }
}