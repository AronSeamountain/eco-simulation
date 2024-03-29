﻿using UnityEngine;

namespace Animal
{
  public sealed class HealthDelegate
  {
    public delegate void HealthChanged(float health, float maxHealth);

    private const float MaxHealth = 100;
    private float _health;

    public HealthChanged HealthChangedListeners;

    public HealthDelegate()
    {
      Health = 100;
    }

    public float Health
    {
      get => _health;
      private set => _health = Mathf.Clamp(value, 0f, MaxHealth);
    }

    public float GetMaxHealth()
    {
      return MaxHealth;
    }

    public void DecreaseHealth(float decrease)
    {
      Health -= decrease;
      HealthChangedListeners?.Invoke(Health, MaxHealth);
    }

    public void IncreaseHealth(int increase)
    {
      Health += increase;
      HealthChangedListeners?.Invoke(Health, MaxHealth);
    }

    public void ResetHealth()
    {
      Health = MaxHealth;
      HealthChangedListeners?.Invoke(Health, MaxHealth);
    }
  }
}