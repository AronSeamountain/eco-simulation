using Core;

namespace Foods.Plants.PlantStates
{
  public sealed class SeedState : IState<PlantState>
  {
    private readonly Plant _plant;

    public SeedState(Plant plant)
    {
      _plant = plant;
    }

    public PlantState GetStateEnum()
    {
      return PlantState.Seed;
    }

    public void Enter()
    {
      _plant.ShowAsSeed();
      _plant.Reset();
    }

    public PlantState Execute()
    {
      if (_plant.LeaveSeedState) return PlantState.Grow;
      return PlantState.Seed;
    }

    public void Exit()
    {
    }
  }
}