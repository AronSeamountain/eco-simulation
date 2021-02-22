using UnityEngine;

public sealed class HealthDelegate
{
  public delegate void HealthChanged(int health, int maxHealth);

  private const int MaxHealth = 100;

  public HealthChanged HealthChangedListeners;
  private int _health;

  public HealthDelegate()
  {
    Health = 100;
  }

  public int GetMaxHealth()
  {
    return MaxHealth;
  }

  public int Health
  {
    get => _health;
    private set => _health = Mathf.Clamp(value, 0, MaxHealth);
  }

  public void DecreaseHealth(int decrease)
  {
    Health -= decrease;
    HealthChangedListeners?.Invoke(Health, MaxHealth);
  }
}