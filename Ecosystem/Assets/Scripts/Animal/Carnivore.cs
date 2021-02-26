using System.Collections.Generic;
using Animal.AnimalStates;
using Animal.Managers;
using Core;
using Utils;

namespace Animal
{
  public sealed class Carnivore : AbstractAnimal
  {
    private const float Range = 10;
    private const float EatingRange = 2f;
    public Herbivore Target { get; private set; }

    private void OnPreyFound(Herbivore herbivore)
    {
      Target = herbivore;
    }

    protected override List<IState<AnimalState>> GetStates(FoodManager fManager)
    {
      fManager.PreyFoundListeners += OnPreyFound;
      return new List<IState<AnimalState>>
      {
        new DeadState(this),
        new WanderState(this),
        new PursueWaterState(this),
        new BirthState(this),
        new HuntState(this),
        new PursueMateState(this),
        new EatState(this),
        new DrinkState(this)
      };
    }

    public bool ShouldHunt(Herbivore carnivoreTarget)
    {
      return Vector3Util.InRange(gameObject, carnivoreTarget.gameObject, Range);
    }

    private bool IsInRange(Herbivore carnivoreTarget)
    {
      return Vector3Util.InRange(gameObject, carnivoreTarget.gameObject, EatingRange);
    }

    public void TakeABiteFromHerbivore(Herbivore carnivoreTarget)
    {
      if (IsInRange(carnivoreTarget))
      {
        carnivoreTarget.TakeDamage();
        _nourishmentDelegate.Saturation++;
      }
    }
  }
}