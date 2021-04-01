﻿using System.Collections.Generic;
using Animal.AnimalStates;
using Animal.Managers;
using Core;
using UnityEngine;
using Utils;

namespace Animal
{
  public sealed class Carnivore : AbstractAnimal
  {
    private const float HuntRange = 15;
    public const float EatingRange = 2f;
    private const float baseAttackDamage = 2;
    private bool _animalOfSameType;

    private bool _hearsHerbivore;
    private Texture _tex;
    private float attackDamage = baseAttackDamage;
    public Herbivore Target { get; set; }
    public bool HasTargetSet => Target != null;

    public bool IsHunting { get; set; }

    private void OnPreyFound(Herbivore herbivore)
    {
      if (Target.DoesNotExist())
        Target = herbivore;

      if (IsCloserTarget(herbivore)) Target = herbivore;
    }

    private bool IsCloserTarget(Herbivore closerTarget)
    {
      var newTarget = Vector3Util.Distance(gameObject, closerTarget.gameObject);
      var oldTarget = Vector3Util.Distance(gameObject, Target.gameObject);
      return newTarget < oldTarget;
    }

    protected override void RenderAnimalSpecificColors()
    {
      if (Gender == Gender.Male)
      {
        _tex = Resources.Load("Wolf_White_COL_1k") as Texture;
        meshRenderer.material.SetTexture("Texture2D_animal_texture", _tex);
      }
      else
      {
        _tex = Resources.Load<Texture2D>("Wolf_COL_1k");
        meshRenderer.material.SetTexture("Texture2D_animal_texture", _tex);
      }
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
        new FleeState(this),
        new IdleState(this),
        new SearchWorldState(this)
      };
    }

    public bool ShouldHunt(Herbivore carnivoreTarget)
    {
      if (!carnivoreTarget || !_nourishmentDelegate.IsHungry || !carnivoreTarget.CanBeEaten()) return false;
      return Vector3Util.InRange(gameObject, carnivoreTarget.gameObject, HuntRange);
    }

    public void AttackTarget(Herbivore carnivoreTarget)
    {
      carnivoreTarget.TakeDamage(attackDamage);
    }

    protected override void OnAnimalHeard(AbstractAnimal animal)
    {
      _hearsHerbivore = animal.IsHerbivore;
      if (_hearsHerbivore) Turn(animal);
    }

    protected override void OnEnemySeen(AbstractAnimal animal)
    {
      //To be implemented when carnivore has an enemy.
    }

    protected override void IncreaseStaminaIfNotRunning()
    {
      if (!IsHunting && Alive) _staminaDelegate.IncreaseStamina(3);
    }

    protected override void DecreaseStaminaIfRunning()
    {
      if (IsRunning && IsHunting) _staminaDelegate.DecreaseStamina(7);
    }

    public override void UpdateScale()
    {
      base.UpdateScale();
      attackDamage = baseAttackDamage * SizeModifier;
    }

    /**
     * Wolfs should get thirsty/hungry slower than rabbits because they are bigger.
     */
    public override float GetNourishmentDecreaseFactor()
    {
      var sizeCubed = SizeModifier * SizeModifier * SizeModifier;
      return sizeCubed + SpeedModifier * SpeedModifier / 1.5f;
    }
  }
}