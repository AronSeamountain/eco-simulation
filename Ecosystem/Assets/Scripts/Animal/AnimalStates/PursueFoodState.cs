using System.Collections.Generic;
using System.Linq;
using Animal.Managers;
using Core;
using Foods;
using UnityEngine;
using Utils;

namespace Animal.AnimalStates
{
  /// <summary>
  ///   An animal state that chases the closest food the animal is aware of. Enters wander state when the animal isn't aware
  ///   of any food sources.
  /// </summary>
  public sealed class PursueFoodState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;
    private FoodManager.FoodMemory _foodTarget;
    private bool _knownFoodTargetsChanged;

    public PursueFoodState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.PursueFood;
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
      if (!_animal.IsHungry) return AnimalState.Wander;
      if (_animal.EnemyToFleeFrom.Exists()) return AnimalState.Flee;
      if (_animal.IsThirsty && !_animal.KnowsWaterLocation && !_animal.IsHungry) return AnimalState.SearchWorld;

      // A new food source has been found. Change the food target to the closest food.
      if (_knownFoodTargetsChanged)
      {
        _knownFoodTargetsChanged = false;
        if (!_animal.KnownFoods.Any()) return AnimalState.Wander;

        _foodTarget = GetClosestFood();
        _animal.GoTo(_foodTarget.Position);
      }
      
      if (_foodTarget != null)
      {
        var position = _animal.transform.position;
        var closestPoint = _foodTarget.Food.foodCollider.ClosestPoint(position);
        var reachesFood = Vector3.Distance(position, closestPoint) < _animal.Reach;
        if (reachesFood)
        {
          if (!_foodTarget.Food.CanBeEaten())
          {
            _animal.Forget(_foodTarget);
            //wait for food to mature
            return AnimalState.Idle;
          }

          
          if (_foodTarget.Food.Exists())
          {
            _animal.FoodAboutTooEat = _foodTarget.Food;
            _animal.Forget(_foodTarget);
            _foodTarget = null;
            return AnimalState.Eat;
          }

          _animal.Forget(_foodTarget);
          _foodTarget = null;
        }

        return AnimalState.PursueFood;
      }

      return AnimalState.Wander;
    }


    public void Exit()
    {
    }

    /// <summary>
    ///   Returns the closest food the animal knows about. Returns null if it knows no foods.
    /// </summary>
    /// <param name="animal">The animal.</param>
    /// <returns>The closest food.</returns>
    private FoodManager.FoodMemory GetClosestFood()
    {
      var foods = _animal.KnownFoods;


      return foods.OrderBy(f => Vector3Util.Distance(_animal.transform.position, f.Position)).First();
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