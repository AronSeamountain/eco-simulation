public sealed class HealthDelegate
{
  public HealthDelegate()
  {
    Health = 100;
  }
  
  public int Saturation { get; }
  public int Hydration { get; }
  public int Health { get; set; }

  public void DecreaseHealth()
  {
    if (Saturation < 10 && Hydration < 10)
    {
      Health--;
    }
  }
  
}