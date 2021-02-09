namespace Foods
{
  public interface IEatable
  {
    int Saturation();

    void Consume();

    FoodType FoodType();
  }
}