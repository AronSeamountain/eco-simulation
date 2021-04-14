namespace Foods
{
  /// <summary>
  ///   A weird food ball.
  /// </summary>
  public sealed class FoodBall : AbstractFood
  {
    private void Start()
    {
      MaxSaturation = 100;
      Saturation = 50;
    }

    protected override void FoodFullyConsumed()
    {
      Destroy(gameObject);
    }

    public override bool CanBeEaten()
    {
      return Saturation > 0;
    }

    public override bool CanBeEatenSoon()
    {
      return Saturation > 0;
    }
  }
}