using UnityEngine;

namespace Foods
{
  public abstract class AbstractFood : MonoBehaviour
  {
    [SerializeField] private FoodType foodType;
    private int _saturation;

    public int Saturation
    {
      get => _saturation;
      protected set => _saturation = Mathf.Clamp(value, 0, MaxSaturation);
    }

    public int MaxSaturation { get; protected set; }

    /// <summary>
    ///   Attempt to eat amount of the food. Will return the amount of food that could be consumed. For example, if the food
    ///   only has 10 "food points" left, and one attempts to eat 100 "food points", it will return 10.
    /// </summary>
    /// <param name="amount">The amount of food points to attempt to eat.</param>
    /// <returns>The amount of food point that was eaten.</returns>
    public int Consume(int amount)
    {
      int consumedFood;

      if (Saturation >= amount)
      {
        // Eat partially
        Saturation -= amount;
        consumedFood = amount;
      }
      else
      {
        // Eat whole food
        consumedFood = Saturation;
        Saturation = 0;
      }

      if (Saturation <= 0)
        FoodFullyConsumed();

      return consumedFood;
    }

    protected abstract void FoodFullyConsumed();

    public FoodType FoodType()
    {
      return foodType;
    }

    public abstract bool CanBeEaten();
  }
}