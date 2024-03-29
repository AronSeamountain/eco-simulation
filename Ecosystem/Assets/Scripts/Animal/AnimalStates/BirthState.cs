﻿using Core;

namespace Animal.AnimalStates
{
  public sealed class BirthState : IState<AnimalState>
  {
    private readonly AbstractAnimal _animal;
    private AbstractAnimal _father;

    public BirthState(AbstractAnimal animal)
    {
      _animal = animal;
      _father = animal.LastMaleMate;
    }

    public AnimalState GetStateEnum()
    {
      return AnimalState.Birth;
    }

    public void Enter()
    {
      _father = _animal.LastMaleMate;
      _animal.IsRunning = false;
    }

    public AnimalState Execute()
    {
      _animal.StopMoving();
      if (_animal.MultipleChildren)
        _animal.SpawnMultipleChildren(_father);
      else
        _animal.SpawnChild(_father);

      return AnimalState.Wander;
    }

    public void Exit()
    {
    }
  }
}