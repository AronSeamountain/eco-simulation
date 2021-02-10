using UnityEngine;

namespace Foods
{
  public class Food : MonoBehaviour, IEatable
  {
    [SerializeField] protected int saturation;
    [SerializeField] private FoodType foodType;

    public int Saturation()
    {
      return saturation;
    }

    public void Consume()
    {
      Destroy(gameObject);
    }

    public FoodType FoodType()
    {
      return foodType;
    }
  }
}