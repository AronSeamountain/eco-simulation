namespace Foods.Plants.PlantStates
{
  public sealed class GrowState : IGenericState<Plant, PlantState>
  {
    public PlantState GetStateEnum()
    {
      return PlantState.Grow;
    }

    public void Enter(Plant plant)
    {
      plant.SetGrowingMaterial();
    }

    public PlantState Execute(Plant plant)
    {
      if (plant.Saturation() >= plant.MaxSaturation) return PlantState.Mature;

      return PlantState.Grow;
    }

    public void Exit(Plant plant)
    {
    }
  }
}