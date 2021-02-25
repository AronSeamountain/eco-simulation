using System;
using Animal.AnimalStates;
using UnityEngine;

namespace Animal.Managers
{
  public class AnimationManager : MonoBehaviour
  {
    [SerializeField] private Animator animator;
    private static readonly int State = Animator.StringToHash("State");
    private const int Birth = 0;
    private const int Dead = 1;
    private const int Wander = 2;
    private const int Pursue = 3;

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
        case AnimalState.Hunt:
          animator.SetInteger(State, Pursue);
          break;
        //TODO add another state "idle" and include it in controller
        //TODO as well as in the code to animate the rabbit standing still
        default:
          throw new ArgumentOutOfRangeException(nameof(state), state, null);
      }
    }
  }
}