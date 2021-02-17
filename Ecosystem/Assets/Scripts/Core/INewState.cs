using System;

namespace Animal
{
  public interface INewState<TEnum> where TEnum : Enum
  {
    /// <summary>
    ///   Gets enum correlating to the state.
    /// </summary>
    /// <returns>The correlating to enum.</returns>
    TEnum GetStateEnum();

    /// <summary>
    ///   Setups the state.
    /// </summary>
    /// <param name="obj">The object of the state.</param>
    void Enter();

    /// <summary>
    ///   Gets called for every frame, should be used as Unitys Update() method.
    /// </summary>
    /// <param name="obj">The object of the state.</param>
    TEnum Execute();

    /// <summary>
    ///   Exits the state and cleans up after itself if needed.
    /// </summary>
    /// <param name="obj">The object of the state.</param>
    void Exit();
  }
}