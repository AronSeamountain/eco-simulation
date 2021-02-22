using UnityEngine;

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

  public void DecreaseHealth(int decrease)
  {
    Health -= decrease;
    HealthChangedListeners?.Invoke(Health, MaxHealth);
  }
}