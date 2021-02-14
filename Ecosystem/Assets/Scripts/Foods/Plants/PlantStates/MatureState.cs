namespace Foods.Plants.PlantStates
{
  public sealed class MatureState : IGenericState<Plant, PlantState>
  {
    public PlantState GetStateEnum()
    {
      return PlantState.Mature;
    }

    public void Enter(Plant plant)
    {
      plant.ShowAsMature();
    }

    public PlantState Execute(Plant plant)
    {
      if (plant.Saturation <= 0) return PlantState.Seed;
      return PlantState.Mature;
    }

    public void Exit(Plant plant)
    {
    }
  }
}