using Core;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace Animal
{
  /// <summary>
  ///   A delegate for something that needs to eat and drink.
  /// </summary>
  public sealed class NourishmentDelegate : ITickable
  {
    public delegate void HydrationChanged(float hydration, float maxHydration);

    public delegate void SaturationChanged(float saturation, float maxSaturation);

    /// <summary>
    ///   The value for which the animal is considered hungry.
    /// </summary>
    private float HungrySaturationLevel { get; set; } = 50;

    /// <summary>
    ///   The value for which the animal is considered thirsty.
    /// </summary>
    private float ThirstyHydrationLevel { get; set; }= 50;

    private float _hydration;
    private float _saturation;
    public HydrationChanged HydrationChangedListeners;
    public SaturationChanged SaturationChangedListeners;

    public NourishmentDelegate()
    {
      Saturation = 25;
      Hydration = 25;
    }

    public float SaturationFromFull()
    {
      return MaxHydration - Hydration;
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
    public float MaxHydration { get; private set; }
    public float MaxSaturation { get; private set; }

    public bool SaturationIsFull()
    {
      return ((MaxSaturation - Saturation) >= 5);
    }

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
      ThirstyHydrationLevel = maxValue / 2;
      HungrySaturationLevel = maxValue / 2;
    }

    private void SaturationInvoker()
    {
      SaturationChangedListeners?.Invoke(Saturation, MaxSaturation);
    }

    private void HydrationInvoker()
    {
      HydrationChangedListeners?.Invoke(Hydration, MaxHydration);
    }
  }
}