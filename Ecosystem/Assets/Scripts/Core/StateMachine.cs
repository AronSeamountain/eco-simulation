using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Core
{
  /// <summary>
  ///   A state machine container that holds a set of states and can retrieve instances of the states given a matching enum.
  /// </summary>
  /// <typeparam name="TStateEnum">The enums for the state machine.</typeparam>
  public sealed class StateMachine<TStateEnum> where TStateEnum : Enum
  {
    public delegate void StateChanged(TStateEnum state);

    private readonly IList<IState<TStateEnum>> _states;
    private IState<TStateEnum> _currentState;
    public StateChanged StateChangedListeners;

    /// <summary>
    ///   Creates a new state machine. Enters the start state.
    /// </summary>
    /// <param name="stateList">The list of states.</param>
    /// <param name="startState">The start state.</param>
    public StateMachine(IList<IState<TStateEnum>> stateList, TStateEnum startState)
    {
      _states = Objects.RequireNonNull(stateList);
      _currentState = GetCorrelatingState(startState);
      _currentState.Enter();
      StateChangedListeners?.Invoke(_currentState.GetStateEnum());
    }

    public void Execute()
    {
      var newState = _currentState.Execute();
      var sameState = EqualityComparer<TStateEnum>.Default.Equals(newState, _currentState.GetStateEnum());
      if (sameState) return;

      _currentState.Exit();
      _currentState = GetCorrelatingState(newState);
      _currentState.Enter();
      StateChangedListeners?.Invoke(_currentState.GetStateEnum());
    }

    /// <summary>
    ///   Gets the state with the provided state enum.
    /// </summary>
    /// <param name="stateEnum">The state to get the state instance from.</param>
    /// <returns>The state correlating to the state enum.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the state machine has no state for the provided state enum.</exception>
    private IState<TStateEnum> GetCorrelatingState(TStateEnum stateEnum)
    {
      foreach (var state in _states)
      {
        if (state.GetStateEnum().Equals(stateEnum))
        {
          return state;
        }
      }

      throw new ArgumentOutOfRangeException(nameof(stateEnum), stateEnum,
        "State machine does not contain a " + stateEnum + " state");
    }

    public TStateEnum GetCurrentStateEnum()
    {
      return _currentState.GetStateEnum();
    }
  }
}