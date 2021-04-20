using System;
using Foods;
using UnityEngine;
using Utils;

namespace Animal.Sensor
{
  /// <summary>
  ///   Scans for food and calls a delegate when food is found.
  /// </summary>
  public sealed class Vision : MonoBehaviour
  {
    public delegate void AnimalFound(AbstractAnimal animal);

    /// <summary>
    ///   Gets invoked when a herbivore discovers a carnivore
    /// </summary>
    /// <param name="animal">The spotted carnivore</param>
    public delegate void EnemySeen(Carnivore animal);

    /// <summary>
    ///   Gets invoked when a food is found.
    /// </summary>
    /// <param name="food">The found that was just found.</param>
    public delegate void FoodFound(AbstractFood food);

    /// <summary>
    ///   Gets invoked when a carnivore finds an animal to eat
    /// </summary>
    /// <param name="animal"></param>
    public delegate void PreyFound(Herbivore animal);

    /// <summary>
    ///   Gets invoked when water is found.
    /// </summary>
    /// <param name="water">The water that was just found.</param>
    public delegate void WaterFound(Water water);

    [SerializeField] private Transform eyesTransform;
    private int _maxHeight = 5;
    private int _maxLength = 40;
    private int _maxWidth = 10;
    private int _height;
    private int _length;
    private int _width;
    public AnimalFound AnimalFoundListeners;
    public EnemySeen EnemySeenListeners;
    public FoodFound FoodFoundListeners;
    public PreyFound PreyFoundListeners;
    public WaterFound WaterFoundListeners;

    /// <summary>
    /// Sets the size to be the given Percentage of the max size.
    /// </summary>
    /// <param name="percecntage"></param>
    public void SetLengthPercentage(float percecntage)
    {
      Length =  (int) (_maxLength * percecntage);
    }

    private int Height
    {
      get => _height;
      set
      {
        _height = Mathf.Clamp(value, 0, int.MaxValue);
        AdjustScaleAndPosition();
      }
    }

    private int Length
    {
      get => _length;
      set
      {
        _length = Mathf.Clamp(value, 0, int.MaxValue);
        AdjustScaleAndPosition();
      }
    }

    private int Width
    {
      get => _width;
      set
      {
        _width = Mathf.Clamp(value, 0, int.MaxValue);
        AdjustScaleAndPosition();
      }
    }

    private void Start()
    {
      Height = _maxHeight;
      Width = _maxWidth;
      AdjustScaleAndPosition();
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.GetComponent<AbstractFood>() is AbstractFood food && food.CanBeEaten())
        FoodFoundListeners?.Invoke(food);

      if (other.GetComponent<AbstractFood>() is AbstractFood potentialFood && potentialFood.CanBeEatenSoon())
        FoodFoundListeners?.Invoke(potentialFood);

      if (other.GetComponent<Herbivore>() is Herbivore animal && animal.CanBeEaten())
        PreyFoundListeners?.Invoke(animal);

      if (other.GetComponent<Water>() is Water water)
        WaterFoundListeners?.Invoke(water);

      if (other.GetComponent<AbstractAnimal>() is AbstractAnimal foundAnimal)
        AnimalFoundListeners?.Invoke(foundAnimal);

      if (other.GetComponent<AbstractAnimal>() is Carnivore carnivore)
        EnemySeenListeners?.Invoke(carnivore);
    }

    /// <summary>
    ///   Checks if the visual detector can see the provided object.
    /// </summary>
    /// <param name="objectToSee">The object to check if can be seen.</param>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <returns>True if it can see the provided object.</returns>
    private bool CanSee<T>(T objectToSee) where T : MonoBehaviour
    {
      throw new NotImplementedException("Update me to check layers correctly!!!!");

      var dirToObject = objectToSee.transform.position - eyesTransform.position;
      var raycastHitSomething =
        Physics.Raycast(eyesTransform.position, dirToObject, out var hitObject, Length, RayCastUtil.CastableLayers);

      if (raycastHitSomething)
        if (hitObject.transform.GetComponent<T>() is T hitObjectOfTypeT)
          if (hitObjectOfTypeT == objectToSee)
          {
            Debug.DrawRay(eyesTransform.position, dirToObject, Color.green, 5);
            return true;
          }

      Debug.DrawRay(eyesTransform.position, dirToObject, Color.red, 0.5f);
      return false;
    }

    /// <summary>
    ///   Scales the detection area and repositions it correctly.
    /// </summary>
    private void AdjustScaleAndPosition()
    {
      transform.localScale = new Vector3(Width, Height, Length);
      var centerOffset = new Vector3(0, -1, Length / 2 - 0.5f);
      transform.localPosition = eyesTransform.localPosition + centerOffset;
    }
  }
}