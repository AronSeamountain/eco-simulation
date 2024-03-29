using System;
using System.Collections.Generic;
using Animal.AnimalStates;
using Animal.Managers;
using Core;
using Foods;
using UnityEngine;
using Utils;

namespace Animal
{
  public sealed class Herbivore : AbstractAnimal
  {
    private const float SafeDistance = 15f;
    private Texture _tex;
    public override float RunningSpeedFactor { get; } = 3f;

    protected override void RenderAnimalSpecificColors()
    {
      if (Gender == Gender.Male)
      {
        _tex = Resources.Load("Rabbit_White_COL_1k") as Texture;
        meshRenderer.material.SetTexture("Texture2D_animal_texture",_tex);
      }
      else
      {
        _tex = Resources.Load<Texture2D>("Rabbit_COL_1k");
        meshRenderer.material.SetTexture("Texture2D_animal_texture",_tex);
      }
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
        new IdleState(this),
        new SearchWorldState(this)
      };
    }

    public bool CanBeEaten()
    {
      return true;
    }

    public void TakeDamage(float damage)
    {
      _healthDelegate.DecreaseHealth(damage);
      if (Dead)
      {
        DeathCause = "eaten";
      }
    }

    private bool WolfDiscovered(AbstractAnimal animal)
    {
      return animal.IsCarnivore;
    }

    protected override void OnAnimalHeard(AbstractAnimal animal)
    {
      if(WolfDiscovered(animal))
      {
        EnemyToFleeFrom = animal;
      }
    }

    protected override void OnEnemySeen(AbstractAnimal animal)
    {
      if(WolfDiscovered(animal))
      {
        EnemyToFleeFrom = animal;
      }
    }


    public override bool SafeDistanceFromEnemy()
    {
      if (EnemyToFleeFrom.Exists())
      {
        var distance = Vector3.Distance(gameObject.transform.position, EnemyToFleeFrom.transform.position);
        return SafeDistance < distance;
      }

      return false;
    }

    protected override void IncreaseStaminaIfNotRunning()
    {
      if (!EnemyToFleeFrom && Alive) _staminaDelegate.IncreaseStamina(5);
    }

    protected override void DecreaseStaminaIfRunning()
    {
      if (IsRunning && EnemyToFleeFrom) _staminaDelegate.DecreaseStamina(4);
    }
    
    public override float GetHydrationDecreaseAmountPerHour(float decreaseFactor)
    {
      return decreaseFactor / 3.5f;
    }
    
    public override float GetSaturationDecreaseAmountPerHour(float decreaseFactor)
    {
      return decreaseFactor / 3f;
    }
    public override float GetBiteSize()
    {
      return Math.Min(160 * SizeModifier * SizeModifier,
        _nourishmentDelegate.SaturationFromFull());
    }
  }
}