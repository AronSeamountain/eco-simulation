using System;
using Animal.AnimalStates;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private NavMeshAgent agent;
    private static readonly int AnimationSpeed = Animator.StringToHash("AnimationSpeed");

    public void ReceiveState(AnimalState state)
    {
      switch (state)
      {
        case AnimalState.Birth:
          animator.SetInteger(State, Birth);
          animator.SetFloat(AnimationSpeed, 1);
          break;
        case AnimalState.Dead:
          animator.SetInteger(State, Dead);
          animator.SetFloat(AnimationSpeed, 1);
          break;
        case AnimalState.Wander:
          animator.SetInteger(State, Wander);
          agent.speed = 1;
          animator.SetFloat(AnimationSpeed, 1);
          break;
        case AnimalState.PursueFood:
        case AnimalState.PursueMate:
        case AnimalState.PursueWater:
        case AnimalState.Hunt:
          animator.SetInteger(State, Pursue);
          agent.speed = 4;
          animator.SetFloat(AnimationSpeed, 1.5f);
          break;
        case AnimalState.Eat:
        case AnimalState.Drink:
          animator.SetInteger(State, Idle);
          animator.SetFloat(AnimationSpeed, 2.5f);
          break;
        case AnimalState.Idle:  
          animator.SetInteger(State, Idle);
          animator.SetFloat(AnimationSpeed, 1);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(state), state, null);
      }
    }

    public void AnimalSound()
    {
      audioSource.Play();
    }
  }
}