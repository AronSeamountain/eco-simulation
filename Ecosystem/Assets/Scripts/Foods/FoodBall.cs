namespace Foods
{
  /// <summary>
  ///   A weird food ball.
  /// </summary>
  public sealed class FoodBall : AbstractFood
  {
    private void Start()
    {
      Saturation = 50;
    }

    public override bool CanBeEaten()
    {
      return Saturation > 0;
    }
  }
}