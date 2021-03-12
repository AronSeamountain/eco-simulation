using Core;
using Foods;
using UnityEngine;

namespace Animal.AnimalStates
{
  public class DrinkState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;
    private Water _water;

    public DrinkState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Drink;
    }

    public void Enter()
    {
      //retrieve target
      _water = _animal.ClosestKnownWater;
      
      _animal.IsRunning = false;

      //set Color
      _animal.SetMouthColor(Color.blue);
    }

    public AnimalState Execute()
    {
      if (!_animal.Alive) return AnimalState.Dead;
      if (!_water) return AnimalState.Wander;
      if (!_animal.CanDrinkMore()) return AnimalState.Wander;
      if (_animal.EnemyToFleeFrom) return AnimalState.Flee;

      _animal.StopMoving();
      _animal.Drink(_water);

      return AnimalState.Drink;
    }

    //TODO test with corutine and an 'IsBusy' variable that could be used, ex: if IsBusy -> return AnimalState.Eat; else -> run the rest

    public void Exit()
    {
      _animal.FoodAboutTooEat = null;
      _water = null;
    }
  }
}