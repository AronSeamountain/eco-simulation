using System.Linq;
using UnityEngine;

namespace AnimalStates
{
  /// <summary>
  ///   A state for pursuing a single piece of food.
  /// </summary>
  public sealed class PursueFoodState : IState
  {
    private Food _foodTarget;

    /// <summary>
    ///   Whether the animal has a food target.
    ///   TODO: Should be converted to a plain bool like in GoTo movement. Null comparision is expensive.
    /// </summary>
    private bool HasFoodTarget => _foodTarget != null;

    public void Enter(Animal animal)
    {
      _foodTarget = GetClosestFood(animal);

      if (_foodTarget != null)
        animal.GoTo(_foodTarget.transform.position);
    }

    public IState Execute(Animal animal)
    {
      if (!HasFoodTarget) return animal.WanderState;

      var distance = Distance(animal.transform.position, _foodTarget.transform.position);
      if (distance < 2)
      {
        animal.Eat(_foodTarget);
        return animal.WanderState;
      }

      return this;
    }

    public void Exit(Animal animal)
    {
      _foodTarget = null;
    }

    /// <summary>
    ///   Returns the closest food the animal knows about. Returns null if animal is null or it knows no foods.
    /// </summary>
    /// <param name="animal">The animal.</param>
    /// <returns>The closest food.</returns>
    private Food GetClosestFood(Animal animal)
    {
      var foods = animal.GetKnownFoods();
      if (foods.Count < 0) return null;

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
  }
}