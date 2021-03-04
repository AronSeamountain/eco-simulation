﻿using System.Collections.Generic;
using Animal.AnimalStates;
using Animal.Managers;
using Core;
using Utils;

namespace Animal
{
  public sealed class Carnivore : AbstractAnimal
  {
    private const float HuntRange = 15;
    public readonly float EatingRange = 2f;
    private bool _animalOfSameType;

    private bool _isHerbivore;
    public Herbivore Target { get; private set; }

    private void OnPreyFound(Herbivore herbivore)
    {
      Target = herbivore;
    }

    protected override void SetAnimalType()
    {
      Type = AnimalType.Carnivore;
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
        new DrinkState(this),
        new FleeState(this)
      };
    }

    public bool ShouldHunt(Herbivore carnivoreTarget)
    {
      if (!carnivoreTarget || !carnivoreTarget.IsAlive) return false;
      return Vector3Util.InRange(gameObject, carnivoreTarget.gameObject, HuntRange);
    }

    public void TakeABiteFromHerbivore(Herbivore carnivoreTarget)
    {
      carnivoreTarget.TakeDamage();
      _nourishmentDelegate.Saturation++;
    }

    protected override void OnAnimalHeard(AbstractAnimal animal)
    {
      if (animal != null) _isHerbivore = animal.IsHerbivore;
      if (_isHerbivore) Turn(animal);
    }
  }
}