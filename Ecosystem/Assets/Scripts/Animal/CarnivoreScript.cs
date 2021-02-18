using System.Collections.Generic;
using Animal;
using AnimalStates;
using Utils;

public sealed class CarnivoreScript : AbstractAnimal
{
  private const float Range = 10;
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


  public bool ShouldHunt(HerbivoreScript carnivoreTarget)
  {
    return Vector3Util.InRange(gameObject, carnivoreTarget.gameObject, Range);
  }
}