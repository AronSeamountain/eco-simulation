using System;

/// <summary>
///   Represents a generic state.
/// </summary>
/// <typeparam name="T">The type that is altered with the state.</typeparam>
/// <typeparam name="TEnum">The matching enums to the states.</typeparam>
public interface IGenericState<in T, out TEnum> where TEnum : Enum
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
  void Enter(T obj);

  /// <summary>
  ///   Gets called for every frame, should be used as Unitys Update() method.
  /// </summary>
  /// <param name="obj">The object of the state.</param>
  TEnum Execute(T obj);

  /// <summary>
  ///   Exits the state and cleans up after itself if needed.
  /// </summary>
  /// <param name="obj">The object of the state.</param>
  void Exit(T obj);
}