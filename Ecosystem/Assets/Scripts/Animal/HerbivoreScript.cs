using System.Collections.Generic;
using Animal;
using AnimalStates;

public sealed class HerbivoreScript : AbstractAnimal
{
  protected override List<INewState<AnimalState>> GetStates()
  {
    return new List<INewState<AnimalState>>
    {
      new DeadState(this),
      new WanderState(this),
      new PursueWaterState(this),
      new BirthState(this),
      new PursueFoodState(this)
    };
  }
}