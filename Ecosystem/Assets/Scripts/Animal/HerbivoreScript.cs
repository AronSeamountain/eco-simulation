using System.Collections.Generic;
using System.Linq;
using Animal;
using AnimalStates;
using Core;
using UnityEngine;


public sealed class HerbivoreScript: AbstractAnimal
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
