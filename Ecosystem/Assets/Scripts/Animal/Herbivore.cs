using System;
using System.Collections.Generic;
using Animal.AnimalStates;
using Animal.Managers;
using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animal
{
  public class Herbivore : AbstractAnimal
  {
    [SerializeField] private int fertilityTimeInDays = 5;
    private bool _hearsCarnivore;
    private readonly float _safeDistance = 15f;

    protected override void AnimalSetup()
    {
      Type = AnimalType.Herbivore;
      FertilitySetup(fertilityTimeInDays);
    }

    protected override List<IState<AnimalState>> GetStates(FoodManager fManager)
    {
      var pursueFoodState = new PursueFoodState(this);
      fManager.KnownFoodMemoriesChangedListeners += pursueFoodState.OnKnownFoodLocationsChanged;
      return new List<IState<AnimalState>>
      {
        new DeadState(this),
        new WanderState(this),
        new PursueWaterState(this),
        new BirthState(this),
        pursueFoodState,
        new PursueMateState(this),
        new EatState(this),
        new DrinkState(this),
        new FleeState(this),
        new IdleState(this)
      };
    }

    public bool CanBeEaten()
    {
      return true;
    }

    public void TakeDamage(int damage)
    {
      _healthDelegate.DecreaseHealth(damage);
    }

    protected override void OnAnimalHeard(AbstractAnimal animal)
    {
      _hearsCarnivore = animal.IsCarnivore;
      if (_hearsCarnivore) enemyToFleeFrom = animal;
    }

    public override bool SafeDistanceFromEnemy()
    {
      var distance = Vector3.Distance(gameObject.transform.position, enemyToFleeFrom.transform.position);
      return _safeDistance < distance;
    }
  }
}