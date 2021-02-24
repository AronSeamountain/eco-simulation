using UnityEngine;
using UnityEngine.AI;

/// <summary>
///   A movement that moves towards transform. Does NOT stop when the agent has arrived.
/// </summary>
public sealed class GoToMovement : MonoBehaviour
{
  [SerializeField] private NavMeshAgent agent;

  public float SpeedFactor { get; set; } = 1;

  /// <summary>
  ///   Whether the movement is currently in pursuit of travelling to a point.
  /// </summary>
  public bool IsMoving => !agent.isStopped;

  /// <summary>
  ///   Moves the agent to the given destination.
  /// </summary>
  /// <param name="destination"></param>
  public void GoTo(Vector3 destination)
  {
    agent.SetDestination(destination);
    Debug.Log("set destination");
    agent.isStopped = false;
    Debug.Log("SPEED: " + agent.speed);
  }

  /// <summary>
  ///   Stops the movement to the vector.
  /// </summary>
  public void Stop()
  {
    agent.isStopped = true;
  }
}