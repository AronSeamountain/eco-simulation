using UnityEngine;

/// <summary>
///   A movement that moves towards transform.
/// </summary>
public sealed class GoToMovement : MonoBehaviour
{
  [SerializeField] private CharacterController controller;
  [SerializeField] private int movementSpeed;
  private Vector3 _target;

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
  
  public void Stop()
  {
    HasTarget = false;
  }
  
  public bool HasTarget { get; set; }
  
  private void Update()
  {
    if (!HasTarget) return;

    if ((Target - transform.position).magnitude < 1)
    {
      Stop();
      return;
    }
    
    var direction = (Target - transform.position).normalized;
    controller.Move(direction * (movementSpeed * Time.deltaTime));
    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
  }
}