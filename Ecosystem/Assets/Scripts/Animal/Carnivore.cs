using System.Collections.Generic;
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
    private bool _animalOfSameType;
    private float attackDamage = 3;

    private bool _hearsHerbivore;
    public Herbivore Target { get; set; }
    public bool HasTargetSet => Target != null;
    
    public bool IsHunting { get; set; }

    private void OnPreyFound(Herbivore herbivore)
    {
        if(Target.DoesNotExist())
          Target = herbivore;

        var newTarget = Vector3Util.Distance(gameObject, herbivore.gameObject);
        var oldTarget = Vector3Util.Distance(gameObject, Target.gameObject);

        if (newTarget < oldTarget)
          Target = herbivore;
    }

    protected override void RenderAnimalSpecificColors()
    {
      if (Gender == Gender.Male)
        meshRenderer.material.color = new Color(0.12f, 0.15f, 0.18f);
      else
        meshRenderer.material.color = new Color(0.5f, 0.56f, 0.61f);
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
    
    /**
     * Wolfs should get thirsty/hungry slower than rabbits because they are bigger.
     */
    public override float GetNourishmentDecreaseFactor()
    {
      var sizeCubed = SizeModifier * SizeModifier * SizeModifier;
      return  sizeCubed + SpeedModifier * SpeedModifier / 1.5f;
    }
  }
}