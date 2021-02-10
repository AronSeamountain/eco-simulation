using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class StateMachineContainer<StateType, StateEnum,_s> where StateType : GenericState<_s, StateEnum> where _s : MonoBehaviour
  where StateEnum : System.Enum
{
  private IList<StateType> _states;

  public StateMachineContainer(IList<StateType> stateList)
  {
    _states = stateList;
  }

  /// <summary>
  ///   Gets the state with the provided state enum.
  /// </summary>
  /// <param name="stateEnum">The state to get the state instance from.</param>
  /// <returns>The state correlating to the state enum.</returns>
  /// <exception cref="ArgumentOutOfRangeException">If the animal has no state for the provided state enum.</exception>
  private StateType GetCorrelatingState(StateEnum stateEnum)
  {
    var state = _states.First(s => s.GetStateEnum().Equals(stateEnum));
    if (state != null) return state;

    throw new ArgumentOutOfRangeException(nameof(state), stateEnum, null);
  }
}

public interface GenericState<GameObjectInstance, StateEnum> where StateEnum : System.Enum where GameObjectInstance : MonoBehaviour

{
  StateEnum GetStateEnum();

  StateEnum Execute(GameObjectInstance b);

  void Enter(GameObjectInstance b);
  void Exit(GameObjectInstance b);
}