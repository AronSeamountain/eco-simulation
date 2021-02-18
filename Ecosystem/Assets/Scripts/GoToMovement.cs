using UnityEngine;
using Utils;

/// <summary>
///   A movement that moves towards transform.
/// </summary>
public sealed class GoToMovement : MonoBehaviour
{
  [SerializeField] private CharacterController controller;
  private Vector3 _target;
  public int MovementSpeed { get; set; }

  /// <summary>
  ///   The target to go to.
  /// </summary>
  public Vector3 Target
  {
    get => _target;
    set
    {
      _target = value;
      HasTarget = true;
    }
  }

  /// <summary>
  ///   Whether the movement is currently in pursuit of travelling to a point.
  /// </summary>
  public bool HasTarget { get; private set; }

  private void Update()
  {
    if (!HasTarget) return;

    // Check if arrived
    var hasArrived = Vector3Util.InRange(Target, transform.position, 1);
    if (hasArrived)
    {
      Stop();
      return;
    }

    // Move
    var direction = (Target - transform.position).normalized;
    controller.Move(direction * (MovementSpeed * Time.deltaTime));
    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
  }

  /// <summary>
  ///   Stops the movement to the vector.
  /// </summary>
  public void Stop()
  {
    HasTarget = false;
  }
}