using UnityEngine;

namespace Foods
{
  public abstract class AbstractFood : MonoBehaviour
  {
    [SerializeField] private FoodType foodType;
    private bool _canBeEaten;
    public int Saturation { get; protected set; }

    /// <summary>
    ///   Attempt to eat amount of the food. Will return the amount of food that could be consumed. For example, if the food
    ///   only has 10 "food points" left, and one attempts to eat 100 "food points", it will return 10.
    /// </summary>
    /// <param name="amount">The amount of food points to attempt to eat.</param>
    /// <returns>The amount of food point that was eaten.</returns>
    public int Consume(int amount)
    {
      if (Saturation >= amount)
      {
        Saturation -= amount;
        return amount;
      }

      var oldSaturation = Saturation;
      Saturation = 0;
      return oldSaturation;
    }

    public FoodType FoodType()
    {
      return foodType;
    }

    public abstract bool CanBeEaten();
  }
}