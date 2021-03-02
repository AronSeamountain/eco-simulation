using System.Collections.Generic;
using Animal.AnimalStates;
using Animal.Managers;
using Core;
using UnityEngine;

namespace Animal
{
  public class Herbivore : AbstractAnimal
  {
    protected override void SetAnimalType()
    {
      Type = AnimalType.Herbivore;
    }

    private bool _isCarnivore;
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
        new DrinkState(this),
        new FleeState(this)
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
    protected override void OnAnimalHeard(AbstractAnimal animal)
    {
      if (animal != null)
      {
        _isCarnivore = animal.IsCarnivore;
      }
      if (_isCarnivore)
      {
        enemyToFleeFrom = animal;
      }
    }
  }
}