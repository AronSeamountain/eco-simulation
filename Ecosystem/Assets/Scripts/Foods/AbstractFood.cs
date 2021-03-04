using UnityEngine;

namespace Foods
{
  public abstract class AbstractFood : MonoBehaviour, IEatable
  {
    public delegate void SaturationChanged(float saturation);

    [SerializeField] private FoodType foodType;
    private float _saturation;

    public SaturationChanged SaturationChangedListeners;

    public float Saturation
    {
      get => _saturation;
      set
      {
        _saturation = Mathf.Clamp(value, 0, MaxSaturation);
        SaturationChangedListeners?.Invoke(Saturation);
      }
    }

    public int MaxSaturation { get; protected set; }

    /// <summary>
    ///   Attempt to eat amount of the food. Will return the amount of food that could be consumed. For example, if the food
    ///   only has 10 "food points" left, and one attempts to eat 100 "food points", it will return 10.
    /// </summary>
    /// <param name="amount">The amount of food points to attempt to eat.</param>
    /// <returns>The amount of food point that was eaten.</returns>
    public float Consume(float amount)
    {
      float consumedFood;

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