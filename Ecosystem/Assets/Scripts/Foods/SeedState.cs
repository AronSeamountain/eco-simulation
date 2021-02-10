

namespace Foods
{
  public class SeedState : GenericState<Plant,PlantState>
  {
    public PlantState GetStateEnum()
    {
      return PlantState.Seed;
    }

    public void Enter(Plant plant)
    {
    }

    public PlantState Execute(Plant plant)
    {
      if (plant.ShouldGrow) return PlantState.Grow;
      return PlantState.Seed;
    }

    public void Exit(Plant plant)
    {
    }

    public void DayTick(Plant plant)
    {
      plant.IncreaseAge();
    }
  }
}