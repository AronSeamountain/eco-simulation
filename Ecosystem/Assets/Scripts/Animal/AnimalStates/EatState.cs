using Core;
using Foods;
using UnityEngine;

namespace Animal.AnimalStates
{
  public sealed class EatState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;
    private IEatable _food;

    public EatState(AbstractAnimal animal)
    {
      _animal = animal;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Eat;
    }

    public void Enter()
    {
      //retrieve target
      _food = _animal.FoodAboutTooEat;

      _animal.IsRunning = false;

      //Set Color
      _animal.SetMouthColor(Color.green);
    }

    public AnimalState Execute()
    {
      if (!_animal.Alive) return AnimalState.Dead;
      if (_food == null) return AnimalState.Wander;
      if (!_food.CanBeEaten()) return AnimalState.Wander;
      if (!_animal.CanEatMore()) return AnimalState.Wander;
      if (_animal.EnemyToFleeFrom) return AnimalState.Flee;

      _animal.StopMoving();
      _animal.Eat(_food);

      return AnimalState.Eat;
    }

    //TODO test with corutine and an 'IsBusy' variable that could be used, ex: if IsBusy -> return AnimalState.Eat; else -> run the rest

    public void Exit()
    {
      _animal.FoodAboutTooEat = null;
      _food = null;
    }
  }
}