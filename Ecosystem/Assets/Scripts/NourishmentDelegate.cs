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
  private const int HungrySaturationLevel = 50;

  /// <summary>
  ///   The value for which the animal is considered thirsty.
  /// </summary>
  private const int ThirstyHydrationLevel = 50;

  private const int SaturationDecreasePerUnit = 1;
  private const int HydrationDecreasePerUnit = 1;
  private int _hydration;
  private int _saturation;

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

  public int Saturation
  {
    get => _saturation;
    set
    {
      _saturation = Mathf.Clamp(value, 0, MaxSaturation);
      SaturationInvoker();
    }
  }

  public int Hydration
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