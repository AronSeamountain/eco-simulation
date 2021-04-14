using Core;
using UnityEngine;
using Utils;

namespace Animal.AnimalStates
{
  public sealed class HuntState : IState<AnimalState>
  {
    private readonly Carnivore _carnivore;
    private const float EatingRange = 2f;
    private Herbivore _target;
    private Vector3 _targetPoint;
    private readonly Sprite _sp = Resources.Load<Sprite>("blood20px");

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
      _carnivore.IsRunning = true;
      _carnivore.SetSpeed();
      _carnivore.IsHunting = true;
      if (_carnivore.KnowsWaterLocation || _carnivore.HasForgottenWater)
      {
        _carnivore.ForgetWaterLocationForSomeTime();
      }
    }

    public AnimalState Execute()
    {
      if (!_carnivore.Alive) return AnimalState.Dead;
      
      _target = _carnivore.Target;
      if (_target.DoesNotExist())
      {
        _carnivore.Target = null;
        return AnimalState.Wander;
      }
      if (_carnivore.IsThirsty && !_carnivore.KnowsWaterLocation && !_carnivore.IsHungry)
        return AnimalState.SearchWorld;
      if (!_carnivore.ShouldHunt(_target))
        return AnimalState.Wander;
      if (_carnivore.GetStaminaDelegate().StaminaZero && !_target.Dead && !Vector3Util.InRange(_carnivore.gameObject, _carnivore.Target.gameObject, _carnivore.Reach + 2f))
      {
        _carnivore.Target = null;
        _carnivore.GetStaminaDelegate().IncreaseStamina(3);
        return AnimalState.Wander;
      }
      var position = _carnivore.transform.position;
      var closestPoint = _target.animalCollider.ClosestPointOnBounds(position);

      _carnivore.GoTo(closestPoint);
      

      if (!Vector3Util.InRange(_targetPoint,closestPoint,1))
      {
        _carnivore.GoTo(closestPoint);
        _targetPoint = closestPoint; 
      }
      if (Vector3.Distance(position, closestPoint) < _carnivore.Reach && _target.NutritionalValue >= 3f)
      {
        if (!_target.Alive)
        {
          _carnivore.FoodAboutTooEat = _target;
          return AnimalState.Eat;
        } 
        _carnivore.SetMouthSprite(_sp); 
        _carnivore.AttackTarget(_target);
      }

      return AnimalState.Hunt;
    }

    public void Exit()
    {
      _target = null;
      _carnivore.IsHunting = false;
    }
  }
}