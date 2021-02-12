﻿using AnimalStates;

public sealed class BirthState : IState
{
  public AnimalState GetStateEnum()
  {
    return AnimalState.Birth;
  }

  public void Enter(Animal animal)
  {
  }

  public AnimalState Execute(Animal animal)
  {
    animal.SpawnChild();
    return AnimalState.Wander;
  }

  public void Exit(Animal animal)
  {
  }
}