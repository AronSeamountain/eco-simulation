using System.Collections.Generic;
using Animal.AnimalStates;
using Animal.Managers;
using Core;
using UnityEngine;

namespace Animal
{
  public class Herbivore : AbstractAnimal
  {
    [SerializeField] private int fertilityTimeInDays = 5;

    protected override void AnimalSetup()
    {
      Type = AnimalType.Herbivore;
      FertilitySetup(fertilityTimeInDays);
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
  }
}