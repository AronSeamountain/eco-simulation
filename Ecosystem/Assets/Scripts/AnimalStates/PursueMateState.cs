using Core;
using UnityEngine;
using Utils;

namespace AnimalStates
{
  public class PursueMateState : IState<Animal, AnimalState>
  {
    public AnimalState GetStateEnum()
    {
      return AnimalState.PursueMate;
    }

    public void Enter(Animal obj)
    {
    }

    public AnimalState Execute(Animal animal)
    {
      if (animal.ShouldBirth) return AnimalState.Birth;
      if (!animal.IsHungry) return AnimalState.Wander;
      if (!animal.IsAlive) return AnimalState.Dead;

      var mateTarget = animal.GetMateTarget();
      if (mateTarget == null) return AnimalState.Wander;
      if (!mateTarget.Fertile) 
      {
        animal.ClearMateTarget();
        return AnimalState.Wander;
      }
      
      var reachesMate = Vector3Util.InRange(animal.gameObject, mateTarget.gameObject, 2);
      if (reachesMate)
      {
        Debug.Log("Reached Mate");
        mateTarget.ProduceChild(animal);
        animal.ClearMateTarget();
        return AnimalState.Wander;
      }
      else
      {
        animal.GoTo(mateTarget.transform.position);
      }

      return AnimalState.PursueMate;
    }

    public void Exit(Animal obj)
    {
    }

    //Todo add some reasonable conditions
    private bool CanMate()
    {
      return true;
    }
  }
}