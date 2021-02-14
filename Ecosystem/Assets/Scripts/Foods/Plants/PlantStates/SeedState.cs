namespace Foods.Plants.PlantStates
{
  public sealed class SeedState : IGenericState<Plant, PlantState>
  {
    public PlantState GetStateEnum()
    {
      return PlantState.Seed;
    }

    public void Enter(Plant plant)
    {
      plant.ShowAsSeed();
    }

    public PlantState Execute(Plant plant)
    {
      if (plant.ShouldGrow) return PlantState.Grow;
      return PlantState.Seed;
    }

    public void Exit(Plant plant)
    {
    }
  }
}