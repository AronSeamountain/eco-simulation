namespace Foods
{
  public interface IEatable
  {
    /// <summary>
    /// Call when this object is being eaten
    /// </summary>
    /// <param name="amount">The max size of a bite</param>
    /// <returns>the smallest of 'amount' and the food ammount of this object left to eat</returns>
    float Consume(float amount);

    /// <summary>
    /// Call to see if the object can be easten
    /// </summary>
    /// <returns>true if the object can be eaten currently</returns>
    bool CanBeEaten();
  }
}