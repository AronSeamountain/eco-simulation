using Core;
using Foods;
using UnityEngine;
using Utils;

namespace Animal.AnimalStates
{
  public sealed class EatState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;
    private IEatable _food;
    private Sprite _sprite;

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
      //_animal.SetMouthColor(Color.green);
      
      if (_animal.IsCarnivore)
      {
        _sprite = Resources.Load<Sprite>("blood20px");
      }
      else
      {
        _sprite= Resources.Load<Sprite>("leaf10px");
      }
      _animal.SetMouthSprite(_sprite);
    }

    public AnimalState Execute()
    {
      if (!_animal.Alive) return AnimalState.Dead;
      if (_food == null) return AnimalState.Wander;
      if (!_food.CanBeEaten()) return AnimalState.Wander;
      if (!_animal.CanEatMore()) return AnimalState.Wander;
      if (_animal.EnemyToFleeFrom.Exists()) return AnimalState.Flee;

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