using System.Collections.Generic;
using UnityEngine;

public class FoodDetector : MonoBehaviour
{
  public delegate void OnFoodFound(List<Food> food);

  public OnFoodFound OnFoodFoundListeners;

  private List<Food> _availableFoods = new List<Food>();
  
  private void OnTriggerStay(Collider other)
  {
    if (other.GetComponent<Food>() is Food food)
    {
      Debug.Log("Det finns mat :D" + food);
      if (!_availableFoods.Contains(food))
      {
        _availableFoods.Add(food); 
      }
      
      OnFoodFoundListeners?.Invoke(_availableFoods);
    }
  }

  public void Eat(Food food)
  {
    _availableFoods.Remove(food);
    Destroy(food.gameObject);
  }
}