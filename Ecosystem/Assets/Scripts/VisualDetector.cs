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

  [SerializeField] private Transform eyesPosition;

  private int _distance;
  private int _radius;
  public FoodFound FoodFoundListeners;
  public WaterFound WaterFoundListeners;

  private int Distance
  {
    get => _distance;
    set
    {
      _distance = Mathf.Clamp(value, 0, int.MaxValue);
      AdjustScaleAndPosition();
    }
  }

  private int Radius
  {
    get => _radius;
    set
    {
      _radius = Mathf.Clamp(value, 0, int.MaxValue);
      AdjustScaleAndPosition();
    }
  }

  private void Start()
  {
    Radius = 10;
    Distance = 20;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.GetComponent<Food>() is Food food) FoodFoundListeners?.Invoke(food);

    if (other.GetComponent<Water>() is Water water) WaterFoundListeners?.Invoke(water);
  }

  /// <summary>
  ///   Scales the detection area and repositions it correctly.
  /// </summary>
  private void AdjustScaleAndPosition()
  {
    transform.localScale = new Vector3(Radius, Distance, Radius);
    var centerOffset = new Vector3(Radius, 0, Distance);
    transform.localPosition = eyesPosition.localPosition + centerOffset;
  }
}