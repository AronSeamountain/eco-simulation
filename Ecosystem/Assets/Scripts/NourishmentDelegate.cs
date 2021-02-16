using System;
using System.Drawing;
using UnityEditor.Experimental;
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
  private const int HungrySaturationLevel = 50;

  /// <summary>
  ///   The value for which the animal is considered thirsty.
  /// </summary>
  private const int ThirstyHydrationLevel = 50;

  private int _saturationDecreasePerUnit = 1;
  private int _hydrationDecreasePerUnit = 1;

  public void setMoving(bool isMoving, double size)
  {
    if (isMoving)
    {
      var newDecrease = Convert.ToInt32(Math.Pow(size, 3.0));
      _saturationDecreasePerUnit = newDecrease;
      _hydrationDecreasePerUnit  = newDecrease;
    }
    else
    {
      var newDecrease = Convert.ToInt32(Math.Pow(size, (0.75)));
      _saturationDecreasePerUnit = newDecrease;
      _hydrationDecreasePerUnit  = newDecrease;
    }
  }

  private int _hydration;
  private int _saturation;

  public NourishmentChanged NourishmentChangedListeners;

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
      Invoker();
    }
  }

  public int Hydration
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
  private int MaxHydration { get; }
  private int MaxSaturation { get; }

  public void Tick()
  {
    Saturation -= _saturationDecreasePerUnit;
    Hydration -= _hydrationDecreasePerUnit;

    Invoker();
  }

  public void DayTick()
  {
  }

  private void Invoker()
  {
    NourishmentChangedListeners?.Invoke(new NourishmentSnapshot(Saturation, Hydration, MaxSaturation, MaxHydration));
  }
}