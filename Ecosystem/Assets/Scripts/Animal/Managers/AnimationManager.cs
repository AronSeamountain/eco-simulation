using System;
using Animal.AnimalStates;
using UnityEngine;

namespace Animal.Managers
{
  public class AnimationManager : MonoBehaviour
  {
    private const int Birth = 0;
    private const int Dead = 1;
    private const int Wander = 2;
    private const int Pursue = 3;
    private const int Idle = 4;
    private static readonly int State = Animator.StringToHash("State");
    private static readonly int AnimationSpeed = Animator.StringToHash("AnimationSpeed");
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;

    public void ReceiveState(AnimalState state, AbstractAnimal animal)
    {
      float animationSpeed;
      switch (state)
      {
        case AnimalState.Birth:
          SetAnimation(Birth, 1);
          break;
        case AnimalState.Dead:
          SetAnimation(Dead, 1);
          break;
        case AnimalState.Wander:
          animationSpeed = animal.SizeModifier * animal.SpeedModifier * 2 /
                           (animal.SizeModifier + animal.SpeedModifier);
          SetAnimation(Wander, animationSpeed);
          break;
        case AnimalState.PursueFood:
        case AnimalState.PursueMate:
        case AnimalState.PursueWater:
        case AnimalState.SearchWorld:
        case AnimalState.Flee:
        case AnimalState.Hunt:
          animationSpeed = animal.SizeModifier * animal.SpeedModifier * 1.4f * 2 /
                           (animal.SizeModifier + animal.SpeedModifier);
          SetAnimation(Pursue, animationSpeed);
          break;
        case AnimalState.Eat:
        case AnimalState.Drink:
          SetAnimation(Idle, 2.5f);
          break;
        case AnimalState.Idle:
          SetAnimation(Idle, 1);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(state), state, null);
      }
    }

    private void SetAnimation(int state, float animationSpeed)
    {
      animator.SetInteger(State, state);
      animator.SetFloat(AnimationSpeed, animationSpeed);
    }

    public void SetAnimationStaminaZero(AbstractAnimal animal)
    {
      var animationSpeed =
        animal.SizeModifier * animal.SpeedModifier * 2 / (animal.SizeModifier + animal.SpeedModifier);
      animator.SetInteger(State, Wander);
      animator.SetFloat(AnimationSpeed, animationSpeed);
    }

    public void AnimalSound()
    {
      //audioSource.Play();
    }
  }
}