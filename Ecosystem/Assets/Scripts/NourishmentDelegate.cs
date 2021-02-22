using UnityEngine;

/// <summary>
///   A delegate for something that needs to eat and drink.
/// </summary>
public sealed class NourishmentDelegate : ITickable
{
  public delegate void NourishmentChanged(NourishmentSnapshot nourishmentSnapshot);

  public delegate void SaturationChanged(int saturation, int maxSaturation);
  
  public delegate void HydrationChanged(int hydration, int maxHydration);

  /// <summary>
  ///   The value for which the animal is considered hungry.
  /// </summary>
  private const float HungrySaturationLevel = 50;

  /// <summary>
  ///   The value for which the animal is considered thirsty.
  /// </summary>
  private const float ThirstyHydrationLevel = 50;

  public float HydrationDecreasePerUnit { get; set; } = 1;


  public float SaturationDecreasePerUnit { get; set; } = 1;

  private float _hydration;
  private float _saturation;

  public NourishmentChanged NourishmentChangedListeners;
  public SaturationChanged SaturationChangedListeners;
  public HydrationChanged HydrationChangedListeners;

  public NourishmentDelegate()
  {
    Saturation = 25;
    Hydration = 25;
    MaxHydration = 100;
    MaxSaturation = 100;
  }

  public void SetMaxNourishment(float maxValue)
  {
    MaxHydration = maxValue;
    MaxSaturation = maxValue;
  }

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
  public int MaxHydration { get; }
  public int MaxSaturation { get; }

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

  private void SaturationInvoker()
  {
    SaturationChangedListeners?.Invoke(Saturation, MaxSaturation);
  }

  private void HydrationInvoker()
  {
    HydrationChangedListeners?.Invoke(Hydration, MaxHydration);
  }

  private void Invoker()
  {
    NourishmentChangedListeners?.Invoke(new NourishmentSnapshot(Saturation, Hydration, MaxSaturation, MaxHydration));
  }
}