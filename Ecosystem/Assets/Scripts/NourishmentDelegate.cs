using UnityEngine;

/// <summary>
///   A delegate for something that needs to eat and drink.
/// </summary>
public sealed class NourishmentDelegate : ITickable
{
  public delegate void NourishmentChanged(NourishmentSnapshot nourishmentSnapshot);

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

  public NourishmentChanged NourishmentChangedListeners;

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
      Invoker();
    }
  }

  public float Hydration
  {
    get => _hydration;
    set
    {
      _hydration = Mathf.Clamp(value, 0, MaxHydration);
      Invoker();
    }
  }

  public bool IsHungry => Saturation <= HungrySaturationLevel;
  public bool IsThirsty => Hydration <= ThirstyHydrationLevel;
  private float MaxHydration { get; set; }
  private float MaxSaturation { get; set; }

  public void Tick()
  {
    Saturation -= SaturationDecreasePerUnit;
    Hydration -= HydrationDecreasePerUnit;

    Invoker();
  }

  public void DayTick()
  {
  }

  public void SetMaxNourishment(float maxValue)
  {
    MaxHydration = maxValue;
    MaxSaturation = maxValue;
  }

  private void Invoker()
  {
    NourishmentChangedListeners?.Invoke(new NourishmentSnapshot(Saturation, Hydration, MaxSaturation, MaxHydration));
  }
}