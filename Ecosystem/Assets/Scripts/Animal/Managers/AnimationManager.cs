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
    [SerializeField] private Animator animator;

    public void ReceiveState(AnimalState state)
    {
      switch (state)
      {
        case AnimalState.Birth:
          animator.SetInteger(State, Birth);
          break;
        case AnimalState.Dead:
          animator.SetInteger(State, Dead);
          break;
        case AnimalState.Wander:
          animator.SetInteger(State, Wander);
          break;
        case AnimalState.PursueFood:
        case AnimalState.PursueMate:
        case AnimalState.PursueWater:
        case AnimalState.Flee:
        case AnimalState.Hunt:
          animator.SetInteger(State, Pursue);
          break;
        case AnimalState.Eat:
        case AnimalState.Drink:
          animator.SetInteger(State, Idle);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(state), state, null);
      }
    }
  }
}