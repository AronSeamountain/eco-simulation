using Animal;
using UnityEngine;
using Utils;

namespace AnimalStates
{
  public sealed class HuntState : INewState<AnimalState>
  {
    private CarnivoreScript _carnivore;
    private HerbivoreScript _target;
    private const float Range = 10;

    public HuntState(CarnivoreScript carnivore)
    {
      _carnivore = carnivore;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Hunt;
    }

    public void Enter()
    {
      //_carnivore.OnPreyFound(_target);
      _target = _carnivore.Target;
      return;
      var colliders = Physics.OverlapSphere(_carnivore.transform.position, 50);
      foreach (var collider in colliders)
        if (collider.GetComponent<HerbivoreScript>() is HerbivoreScript f)
          _target = f;
          
    }

    public AnimalState Execute()
    {
      if (!_target) return AnimalState.Wander;
      if (!Vector3Util.InRange(_carnivore.gameObject, _target.gameObject, Range)) return AnimalState.Wander;
      _carnivore.GoTo(_target.transform.position);
      return AnimalState.Hunt;
    }

    public void Exit()
    {
      _target = null;
    }

  }
}