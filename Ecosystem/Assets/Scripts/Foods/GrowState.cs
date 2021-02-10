using System;


namespace Foods
{
  public class GrowState : GenericState<Plant,PlantState>
  {
    public PlantState GetStateEnum()
    {
      return PlantState.Grow;
    }

    public void Enter(Plant plant)
    {
      
      plant.Saturation();
      
    }

    public PlantState Execute(Plant plant)
    {
      if (plant.Saturation() >= plant.MaxSaturation()) return PlantState.Mature;
      
      return PlantState.Grow;
    }

    public void Exit(Plant plant)
    {
      throw new NotImplementedException();
    }
  }
}