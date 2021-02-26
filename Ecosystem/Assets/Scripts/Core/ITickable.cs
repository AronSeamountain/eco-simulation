namespace Core
{
  public interface ITickable
  {
    void Tick();
    void DayTick();
  }
}