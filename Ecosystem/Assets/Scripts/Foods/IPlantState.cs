namespace Foods
{
  public interface IPlantState : IGenericState<Plant,PlantState>
  {
    void DayTick(Plant plant);
  }
}