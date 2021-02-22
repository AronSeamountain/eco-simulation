using UnityEngine;

/// <summary>
///   A delegate for something that needs to eat and drink.
/// </summary>
public sealed class NourishmentDelegate : ITickable
{
  public delegate void HydrationChanged(int hydration, int maxHydration);

  public delegate void NourishmentChanged(NourishmentSnapshot nourishmentSnapshot);

  public delegate void SaturationChanged(int saturation, int maxSaturation);

  /// <summary>
  ///   The value for which the animal is considered hungry.
  /// </summary>
  private const float HungrySaturationLevel = 50;

  /// <summary>
  ///   The value for which the animal is considered thirsty.
  /// </summary>
  private const float ThirstyHydrationLevel = 50;

  private float _hydration;
  private float _saturation;
  public HydrationChanged HydrationChangedListeners;
  public SaturationChanged SaturationChangedListeners;

  public NourishmentDelegate()
  {
    Saturation = 25;
    Hydration = 25;
    MaxHydration = 100;
    MaxSaturation = 100;
  }

  public float HydrationDecreasePerUnit { get; set; } = 1;


  public float SaturationDecreasePerUnit { get; set; } = 1;

  public float Saturation
  {
    get => _saturation;
    set
    {
      _saturation = Mathf.Clamp(value, 0, MaxSaturation);
      SaturationInvoker();
    }
  }

  public float Hydration
  {
    get => _hydration;
    set
    {
      _hydration = Mathf.Clamp(value, 0, MaxHydration);
      HydrationInvoker();
    }
  }

  public bool IsHungry => Saturation <= HungrySaturationLevel;
  public bool IsThirsty => Hydration <= ThirstyHydrationLevel;
  public int MaxHydration { get; set; }
  public int MaxSaturation { get; set; }

  public void Tick()
  {
    Saturation -= SaturationDecreasePerUnit;
    SaturationInvoker();

    Hydration -= HydrationDecreasePerUnit;
    HydrationInvoker();
  }

  public void DayTick()
  {
  }

  public void SetMaxNourishment(float maxValue)
  {
    MaxHydration = maxValue;
    MaxSaturation = maxValue;
  }

  private void SaturationInvoker()
  {
    SaturationChangedListeners?.Invoke((int) Saturation, MaxSaturation);
  }

  private void HydrationInvoker()
  {
    HydrationChangedListeners?.Invoke((int) Hydration, MaxHydration);
  }
}