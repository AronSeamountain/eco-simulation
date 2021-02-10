namespace Foods
{
  public interface IPlantState : GenericState<Plant,PlantState>
  {
    void DayTick(Plant plant);
  }
}