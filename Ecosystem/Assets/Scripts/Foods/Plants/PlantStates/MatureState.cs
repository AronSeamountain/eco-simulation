using Core;

namespace Foods.Plants.PlantStates
{
  public sealed class MatureState : IState<PlantState>
  {
    private readonly Plant _plant;

    public MatureState(Plant plant)
    {
      _plant = plant;
    }

    public PlantState GetStateEnum()
    {
      return PlantState.Mature;
    }

    public void Enter()
    {
      _plant.ShowAsMature();
    }

    public PlantState Execute()
    {
      if (_plant.Saturation <= 0) return PlantState.Seed;
      return PlantState.Mature;
    }

    public void Exit()
    {
    }
  }
}