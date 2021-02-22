using System;
using System.Collections.Generic;
using System.Linq;
using Animal;
using Utils;

namespace Core
{
  /// <summary>
  ///   A state machine container that holds a set of states and can retrieve instances of the states given a matching enum.
  /// </summary>
  /// <typeparam name="TEnum">The enums for the state machine.</typeparam>
  public sealed class NewStateMachine<TEnum> where TEnum : Enum
  {
    private readonly IList<INewState<TEnum>> _states;

    public NewStateMachine(IList<INewState<TEnum>> stateList)
    {
      _states = Objects.RequireNonNull(stateList);
    }

    /// <summary>
    ///   Gets the state with the provided state enum.
    /// </summary>
    /// <param name="stateEnum">The state to get the state instance from.</param>
    /// <returns>The state correlating to the state enum.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the state machine has no state for the provided state enum.</exception>
    public INewState<TEnum> GetCorrelatingState(TEnum stateEnum)
    {
      var state = _states.First(s => s.GetStateEnum().Equals(stateEnum));
      if (state != null) return state;

      throw new ArgumentOutOfRangeException(nameof(state), stateEnum, null);
    }
  }
}