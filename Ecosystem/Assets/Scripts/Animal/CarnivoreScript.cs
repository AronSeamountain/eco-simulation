using System.Collections.Generic;
using Animal;
using AnimalStates;
using Utils;

public sealed class CarnivoreScript : AbstractAnimal
{
  private const float Range = 10;
  private const float EatingRange = 2f;
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

  public bool ShouldHunt(HerbivoreScript carnivoreTarget)
  {
    return Vector3Util.InRange(gameObject, carnivoreTarget.gameObject, Range);
  }

  private bool IsInRange(HerbivoreScript carnivoreTarget)
  {
    return Vector3Util.InRange(gameObject, carnivoreTarget.gameObject, EatingRange);
  }

  public void TakeABiteFromHerbivore(HerbivoreScript carnivoreTarget)
  {
    if (IsInRange(carnivoreTarget))
    {
      carnivoreTarget.TakeDamage();
      _nourishmentDelegate.Saturation++;
    }
  }
}