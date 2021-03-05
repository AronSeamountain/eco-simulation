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

    public void ReceiveState(AnimalState state)
    {
      switch (state)
      {
        case AnimalState.Birth:
          SetAnimation(Birth, 1);
          break;
        case AnimalState.Dead:
          SetAnimation(Dead, 1);
          break;
        case AnimalState.Wander:
          SetAnimation(Wander, 1);
          break;
        case AnimalState.PursueFood:
        case AnimalState.PursueMate:
        case AnimalState.PursueWater:
        case AnimalState.Hunt:
          SetAnimation(Pursue, 1.5f);
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

    public void AnimalSound()
    {
      audioSource.Play();
    }
  }
}