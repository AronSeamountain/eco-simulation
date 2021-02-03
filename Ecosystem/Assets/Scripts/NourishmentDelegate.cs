/// <summary>
///   A delegate for something that needs to eat and drink.
/// </summary>
public sealed class NourishmentDelegate
{
  /// <summary>
  ///   The value for which the animal is considered hungry.
  /// </summary>
  private const int HungrySaturationLevel = 50;

  /// <summary>
  ///   The value for which the animal is considered thirsty.
  /// </summary>
  private const int ThirstyHydrationLevel = 50;

  private const int SaturationDecreasePerUnit = 1;
  private const int HydrationDecreasePerUnit = 1;

  /// <summary>
  ///   The amount of time that a "unit" is in.
  /// </summary>
  private const float UnitTimeSeconds = 0.5f;

  private float _unitTicker;

  public NourishmentDelegate()
  {
    Saturation = 25;
    Hydration = 25;
  }

  public int Saturation { get; set; }
  public int Hydration { get; set; }
  public bool IsHungry => Saturation <= HungrySaturationLevel;
  public bool IsThirsty => Hydration <= ThirstyHydrationLevel;

  /// <summary>
  ///   Slowly decrease hunger and thirst with time.
  /// </summary>
  /// <param name="deltaTime">The time since last frame.</param>
  public void Tick(float deltaTime)
  {
    _unitTicker += deltaTime;
    if (_unitTicker >= UnitTimeSeconds)
    {
      _unitTicker = 0;
      Saturation -= SaturationDecreasePerUnit;
      Hydration -= HydrationDecreasePerUnit;
    }
  }
}