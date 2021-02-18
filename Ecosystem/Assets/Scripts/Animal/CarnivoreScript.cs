using System.Collections.Generic;
using Animal;
using AnimalStates;
using UnityEngine;

public sealed class CarnivoreScript : AbstractAnimal
{
  public HerbivoreScript Target { get; private set; }
  
  public void OnPreyFound(HerbivoreScript herbivore)
  {
    Target = herbivore;
  }

  protected override List<INewState<AnimalState>> GetStates(FoodManager fManager)
  {
    fManager.PreyFoundListeners += OnPreyFound;
    return new List<INewState<AnimalState>>
    {
      new DeadState(this),
      new WanderState(this),
      new PursueWaterState(this),
      new BirthState(this),
      new HuntState(this)
    };
  }

  
  
  private bool CanBeEaten(AbstractAnimal animal)
  {
    if (true)
      return true;
  }
  

}