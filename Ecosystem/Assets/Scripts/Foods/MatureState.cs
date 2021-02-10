using System;

namespace Foods
{
  public class MatureState : IPlantState
  {
    public PlantState GetStateEnum()
    {
      throw new NotImplementedException();
    }

    public void Enter(Plant plant)
    {
      throw new NotImplementedException();
    }

    public PlantState Execute(Plant plant)
    {
      throw new NotImplementedException();
    }

    public void Exit(Plant plant)
    {
      throw new NotImplementedException();
    }

    public void DayTick(Plant plant)
    {
      throw new NotImplementedException();
    }
  }
}