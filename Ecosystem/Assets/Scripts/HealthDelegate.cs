public sealed class HealthDelegate
{
  public HealthDelegate()
  {
    Health = 100;
  }

  public int Health { get; private set; }

  public void DecreaseHealth(int decrease)
  {
    Health -= decrease;
  }
}