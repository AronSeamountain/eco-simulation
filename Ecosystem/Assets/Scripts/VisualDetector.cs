using Foods;
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
  public delegate void FoodFound(AbstractFood food);

  /// <summary>
  ///   Gets invoked when water is found.
  /// </summary>
  /// <param name="water">The water that was just found.</param>
  public delegate void WaterFound(Water water);

  [SerializeField] private Transform eyesTransform;
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
    if (other.GetComponent<AbstractFood>() is AbstractFood food && food.CanBeEaten() && CanSee(food))
      FoodFoundListeners?.Invoke(food);

    if (other.GetComponent<Water>() is Water water && CanSee(water))
      WaterFoundListeners?.Invoke(water);
  }

  /// <summary>
  ///   Checks if the visual detector can see the provided object.
  /// </summary>
  /// <param name="objectToSee">The object to check if can be seen.</param>
  /// <typeparam name="T">The type of the object.</typeparam>
  /// <returns>True if it can see the provided object.</returns>
  private bool CanSee<T>(T objectToSee) where T : MonoBehaviour
  {
    var dirToObject = objectToSee.transform.position - eyesTransform.position;
    var raycastHitSomething = Physics.Raycast(eyesTransform.position, dirToObject, out var hitObject);

    if (raycastHitSomething)
      if (hitObject.transform.GetComponent<T>() is T hitObjectOfTypeT)
        if (hitObjectOfTypeT == objectToSee)
        {
          Debug.DrawRay(eyesTransform.position, dirToObject, Color.green, 5);
          return true;
        }

    Debug.DrawRay(eyesTransform.position, dirToObject, Color.red, 5);
    return false;
  }

  /// <summary>
  ///   Scales the detection area and repositions it correctly.
  /// </summary>
  private void AdjustScaleAndPosition()
  {
    transform.localScale = new Vector3(Radius, Distance, Radius);
    var centerOffset = new Vector3(Radius, 0, Distance);
    transform.localPosition = eyesTransform.localPosition + centerOffset;
  }
}