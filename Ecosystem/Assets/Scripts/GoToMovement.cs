using UnityEngine;

/// <summary>
///   A movement that moves towards transform.
/// </summary>
public sealed class GoToMovement : MonoBehaviour
{
  [SerializeField] private CharacterController controller;
  [SerializeField] private int movementSpeed;
  private Transform _target;

  /// <summary>
  ///   The target to go to.
  /// </summary>
  public Transform Target
  {
    get => _target;
    set
    {
      _target = value;
      HasTarget = _target != null;
    }
  }

  public void Stop()
  {
    Target = null;
  }
  
  private bool HasTarget { get; set; }

  private void Start()
  {
    Target = null;
  }

  private void Update()
  {
    if (!HasTarget) return;

    var direction = (Target.transform.position - transform.position).normalized;
    controller.Move(direction * (movementSpeed * Time.deltaTime));
    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
  }
}