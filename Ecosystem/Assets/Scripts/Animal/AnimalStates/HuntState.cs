using Core;
using UnityEngine;
using Utils;

namespace Animal.AnimalStates
{
  public sealed class HuntState : IState<AnimalState>
  {
    private readonly Carnivore _carnivore;
    private Herbivore _target;
    private float _eatingRange;

    public HuntState(Carnivore carnivore)
    {
      _carnivore = carnivore;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Hunt;
    }

    public void Enter()
    {
      _carnivore.SetSpeed(5);
      _target = _carnivore.Target;
      _eatingRange = _target.transform.localScale.x;
    }

    public AnimalState Execute()
    {
      if (!_carnivore.Alive) return AnimalState.Dead;
      if (_carnivore.IsThirsty && !_carnivore.KnowsWaterLocation && !_carnivore.IsHungry)
        return AnimalState.SearchWorld;
      if (!_carnivore.ShouldHunt(_target))
        return AnimalState.Wander;

      if (_target.DoesNotExist())
      {
        _carnivore.Target = null;
        return AnimalState.Wander;
      }

      _carnivore.GoTo(_target.transform.position);
      
      

      if (Vector3Util.InRange(_carnivore, _target, _eatingRange))
      {
        if (!_target.Alive)
        {
          _carnivore.FoodAboutTooEat = _target;
          return AnimalState.Eat;
        }

        _carnivore.SetMouthColor(Color.red);
        _carnivore.AttackTarget(_target);
      }

      return AnimalState.Hunt;
    }

    public void Exit()
    {
      _target = null;
    }
  }
}