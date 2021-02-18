﻿using System.Collections.Generic;
using Animal;
using AnimalStates;

public sealed class HerbivoreScript : AbstractAnimal
{
  protected override List<INewState<AnimalState>> GetStates(FoodManager fManager)
  {
    var pursueFoodState = new PursueFoodState(this);
    fManager.KnownFoodMemoriesChangedListeners += pursueFoodState.OnKnownFoodLocationsChanged;
    return new List<INewState<AnimalState>>
    {
      new DeadState(this),
      new WanderState(this),
      new PursueWaterState(this),
      new BirthState(this),
      pursueFoodState
    };
  }

  public bool CanBeEaten()
  {
    return true;
  }
}