using UnityEngine;

/// <summary>
///   Scans for food and calls a delegate when food is found.
/// </summary>
public sealed class VisualDetector : MonoBehaviour
{
  /// <summary>
  ///   Gets invoked when a food is found.
  /// </summary>
  /// <param name="food">The found that was just found.</param>
  public delegate void FoodFound(Food food);

  /// <summary>
  ///   Gets invoked when water is found.
  /// </summary>
  /// <param name="water">The water that was just found.</param>
  public delegate void WaterFound(Water water);

  public FoodFound FoodFoundListeners;
  public WaterFound WaterFoundListeners;

  private void OnTriggerEnter(Collider other)
  {
    if (other.GetComponent<Food>() is Food food) FoodFoundListeners?.Invoke(food);

    if (other.GetComponent<Water>() is Water water) WaterFoundListeners?.Invoke(water);
  }
}