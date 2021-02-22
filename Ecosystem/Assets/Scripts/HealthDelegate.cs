public sealed class HealthDelegate
{
  public delegate void HealthChanged(int health, int maxHealth);

  private const int MaxHealth = 100;

  public HealthChanged HealthChangedListeners;

  public HealthDelegate()
  {
    Health = 100;
  }

  public int Health { get; set; }

  public void DecreaseHealth(int decrease)
  {
    Health -= decrease;
    HealthChangedListeners?.Invoke(Health, MaxHealth);
  }
}