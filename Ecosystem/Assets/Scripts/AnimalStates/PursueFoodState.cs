using System.Collections.Generic;
using System.Linq;
using Core;
using Foods;
using UnityEngine;
using Utils;

namespace AnimalStates
{
  /// <summary>
  ///   An animal state that chases the closest food the animal is aware of. Enters wander state when the animal isn't aware
  ///   of any food sources.
  /// </summary>
  public sealed class PursueFoodState : IState<Animal, AnimalState>
  {
    private FoodManager.FoodMemory _foodTarget;
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
      if (!animal.IsAlive) return AnimalState.Dead;
      if (animal.ShouldBirth) return AnimalState.Birth;
      if (!animal.IsHungry) return AnimalState.Wander;

      // A new food source has been found. Change the food target to the closest food.
      if (_knownFoodTargetsChanged)
      {
        _knownFoodTargetsChanged = false;
        if (!animal.KnownFoods.Any()) return AnimalState.Wander;

        _foodTarget = GetClosestFood(animal);
        animal.GoTo(_foodTarget.Position);
      }

      // Eat the current food if it can be reached.
      var reachesFood = Vector3Util.InRange(animal.transform.position, _foodTarget.Position, 2);
      if (reachesFood)
      {
        var colliders = Physics.OverlapSphere(animal.transform.position, 2);
        foreach (var collider in colliders)
          if (collider.GetComponent<AbstractFood>() is AbstractFood f)
            if (f == _foodTarget.Food)
            {
              animal.StopMoving();
              animal.Eat(_foodTarget.Food);
              break;
            }

        animal.Forget(_foodTarget);
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
    private FoodManager.FoodMemory GetClosestFood(Animal animal)
    {
      var foods = animal.KnownFoods;


      return foods.OrderBy(f => Vector3Util.Distance(animal.transform.position, f.Position)).First();
    }

    /// <summary>
    ///   Lets the state know that there is potentially a closer food.
    /// </summary>
    /// <param name="foods">The set of foods the animals knows of.</param>
    public void OnKnownFoodLocationsChanged(IEnumerable<FoodManager.FoodMemory> foods)
    {
      _knownFoodTargetsChanged = true;
    }
  }
}