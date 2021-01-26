using UnityEngine;

public class FoodDetector : MonoBehaviour


{
  public delegate void OnFoodFound(Food food);

  public OnFoodFound OnFoodFoundListeners;

  private void OnTriggerEnter(Collider other)
  {
    Debug.Log(other);
    if (other.GetComponent<Food>() is Food food)
    {
      Debug.Log("Hittade mat :D");
      OnFoodFoundListeners?.Invoke(food);
    }
  }
}