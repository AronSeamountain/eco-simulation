using UnityEngine;
using Utils;

/// <summary>
///   A movement that moves towards transform.
/// </summary>
public sealed class GoToMovement : MonoBehaviour
{
  [SerializeField] private CharacterController controller;
  [SerializeField] private int movementSpeed;
  private Vector3 _target;
  public int MovementSpeed { get; set; }

  public float SpeedFactor { get; set; } = 1;

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
    controller.Move(direction * (movementSpeed * SpeedFactor * Time.deltaTime));
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