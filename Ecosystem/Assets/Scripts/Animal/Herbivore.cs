using System.Collections.Generic;
using Animal.AnimalStates;
using Animal.Managers;
using Core;

namespace Animal
{
  public sealed class Herbivore : AbstractAnimal
  {
    protected override void SetAnimalType()
    {
      Specie = AnimalSpecie.Rabbit;
    }
  
    protected override void RenderAnimalSpecificColors()
    {
      if (Gender == Gender.Male)
        genderRenderer.material.color = new UnityEngine.Color(1f, 0.8f, 0.8f);
      else
        genderRenderer.material.color = new UnityEngine.Color(1f, 0.72f, 0.72f);
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

    public void TakeDamage(int damage)
    {
      _healthDelegate.DecreaseHealth(damage);
    }
  }
}