namespace Foods
{
  public interface IEatable
  {
    float Eat(float amount);

    bool CanBeEaten();
  }
}