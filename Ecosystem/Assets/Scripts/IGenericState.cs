using System;
using UnityEngine;

/// <summary>
///   Represents a generic state.
/// </summary>
/// <typeparam name="T">The type that is altered with the state.</typeparam>
/// <typeparam name="TEnum">The matching enums to the states.</typeparam>
public interface IGenericState<in T, out TEnum>
  where T : MonoBehaviour
  where TEnum : Enum
{
  TEnum GetStateEnum();

  void Enter(T obj);

  TEnum Execute(T obj);

  void Exit(T obj);
}