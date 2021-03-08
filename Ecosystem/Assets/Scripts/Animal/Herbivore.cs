using System.Collections.Generic;
using Animal.AnimalStates;
using Animal.Managers;
using Core;
using UnityEngine;

namespace Animal
{
  public sealed class Herbivore : AbstractAnimal
  {
    private const float SafeDistance = 15f;
    [SerializeField] private int fertilityTimeInDays = 5;
    private bool _hearsCarnivore;
    private bool _seesCarnivore;

    protected override void AnimalSetup()
    {
      Species = AnimalSpecies.Rabbit;
      FertilitySetup(fertilityTimeInDays);
    }

    protected override void RenderAnimalSpecificColors()
    {
      if (Gender == Gender.Male)
        meshRenderer.material.color = new Color(1f, 0.8f, 0.8f);
      else
        meshRenderer.material.color = new Color(0.99f, 0.65f, 0.87f);
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
      if (_hearsCarnivore) EnemyToFleeFrom = animal;
    }

    protected override void OnEnemySeen(AbstractAnimal animal)
    {
      _seesCarnivore = animal.IsCarnivore;
      if (_seesCarnivore) EnemyToFleeFrom = animal;
    }

    public override bool SafeDistanceFromEnemy()
    {
      var distance = Vector3.Distance(gameObject.transform.position, EnemyToFleeFrom.transform.position);
      return SafeDistance < distance;
    }
  }
}