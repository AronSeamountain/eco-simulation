﻿using System;
using System.Collections.Generic;
using Animal.AnimalStates;
using Animal.Managers;
using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animal
{
  public class Herbivore : AbstractAnimal
  {
    protected override void SetAnimalType()
    {
      Type = AnimalType.Herbivore;
    }

    protected override List<IState<AnimalState>> GetStates(FoodManager fManager)
    {
      var pursueFoodState = new PursueFoodState(this);
      fManager.KnownFoodMemoriesChangedListeners += pursueFoodState.OnKnownFoodLocationsChanged;
      return new List<IState<AnimalState>>
      {
        new DeadState(this),
        new WanderState(this),
        new PursueWaterState(this),
        new BirthState(this),
        pursueFoodState,
        new PursueMateState(this),
        new EatState(this),
        new DrinkState(this)
      };
    }

    public bool CanBeEaten()
    {
      return true;
    }

    public void TakeDamage()
    {
      _healthDelegate.DecreaseHealth(1);
    }

    public override AbstractAnimal SpawnChild(AbstractAnimal father)
    {
      Herbivore child = (Herbivore) base.SpawnChild(father);
      
      return child;
    }
  }
}