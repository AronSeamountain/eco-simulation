namespace Core
{
  public interface ITickable
  {
    void HourTick();
    void DayTick();
  }
}