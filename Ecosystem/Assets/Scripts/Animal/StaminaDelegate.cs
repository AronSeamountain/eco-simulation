﻿using UnityEngine;

namespace Animal
{
  public sealed class StaminaDelegate
  {
    public delegate void StaminaChanged(float stamina, float maxStamina);

    private const float MaxStamina = 100;
    private float _stamina;

    public StaminaChanged StaminaChangedListeners;
    public StaminaChanged StaminaZeroListeners;

    public StaminaDelegate()
    {
      Stamina = 100;
    }

    public bool StaminaZero => Stamina <= 0;
    public bool HasMaxStamina => Stamina == MaxStamina;

    public float Stamina
    {
      get => _stamina;
      private set => _stamina = Mathf.Clamp(value, 0f, MaxStamina);
    }

    public float GetMaxStamina()
    {
      return MaxStamina;
    }

    public void DecreaseStamina(int decrease)
    {
      Stamina -= decrease;
      StaminaChangedListeners?.Invoke(Stamina, MaxStamina);
      if (StaminaZero) StaminaZeroListeners?.Invoke(Stamina, MaxStamina);
    }

    public void IncreaseStamina(int increase)
    {
      Stamina += increase;
      StaminaChangedListeners?.Invoke(Stamina, MaxStamina);
    }
  }
}