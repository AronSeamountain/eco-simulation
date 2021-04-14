using Core;

namespace Foods.Plants.PlantStates
{
  public sealed class GrowState : IState<PlantState>
  {
    private readonly Plant _plant;

    public GrowState(Plant plant)
    {
      _plant = plant;
    }

    public PlantState GetStateEnum()
    {
      return PlantState.Grow;
    }

    public void Enter()
    {
      _plant.ShowAsGrowing();
    }

    public PlantState Execute()
    {
      if (_plant.Saturation <= 0.1f) return PlantState.Seed;
      if (_plant.Saturation >= _plant.MaxSaturation / 2) return PlantState.Mature;
      return PlantState.Grow;
    }

    public void Exit()
    {
    }
  }
}