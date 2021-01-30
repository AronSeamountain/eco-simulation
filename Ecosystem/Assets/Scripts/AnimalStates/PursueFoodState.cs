using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnimalStates
{
  /// <summary>
  ///   An animal state that chases the closest food the animal is aware of. Enters wander state when the animal isn't aware
  ///   of any food sources.
  /// </summary>
  public sealed class PursueFoodState : IState
  {
    private Food _foodTarget;
    private bool _knownFoodTargetsChanged;

    public AnimalState GetStateEnum()
    {
      return AnimalState.PursueFood;
    }

    public void Enter(Animal animal)
    {
    }

    public AnimalState Execute(Animal animal)
    {
      // A new food source has been found. Change the food target to the closest food.
      if (_knownFoodTargetsChanged)
      {
        _knownFoodTargetsChanged = false;
        _foodTarget = GetClosestFood(animal);
        if (_foodTarget != null) animal.GoTo(_foodTarget.transform.position);
      }

      // No food -> Enter wander state
      if (_foodTarget == null) return AnimalState.Wander;

      // Eat the current food if it can be reached.
      var reachesFood = Distance(animal.transform.position, _foodTarget.transform.position) < 2;
      if (reachesFood)
      {
        animal.Eat(_foodTarget);
        _foodTarget = null;
      }

      return AnimalState.PursueFood;
    }

    public void Exit(Animal animal)
    {
    }

    /// <summary>
    ///   Returns the closest food the animal knows about. Returns null if it knows no foods.
    /// </summary>
    /// <param name="animal">The animal.</param>
    /// <returns>The closest food.</returns>
    private Food GetClosestFood(Animal animal)
    {
      var foods = animal.KnownFoods;
      if (!foods.Any()) return null;

      var animalPos = animal.transform.position;
      return foods.OrderBy(f => Distance(animalPos, f.transform.position)).First();
    }

    /// <summary>
    ///   Gets the distance between two points.
    /// </summary>
    /// <param name="point1">The first point.</param>
    /// <param name="point2">The second point.</param>
    /// <returns>The distance between two points.</returns>
    private float Distance(Vector3 point1, Vector3 point2)
    {
      return (point1 - point2).magnitude;
    }

    /// <summary>
    ///   Lets the state know that there is potentially a closer food.
    /// </summary>
    /// <param name="foods">The set of foods the animals knows of.</param>
    public void OnKnownFoodLocationsChanged(IEnumerable<Food> foods)
    {
      _knownFoodTargetsChanged = true;
    }
  }
}