using UnityEngine;
using UnityEngine.InputSystem;

public class DisplayCharacterStats : MonoBehaviour
{
  [SerializeField] private Camera mainCamera;
  private CameraControls _controls;
  private Animal _target;

  private void Awake()
  {
    _controls = new CameraControls();
    _controls.CameraMovement.Selecting.performed += ClickedChar;
    _controls.CameraMovement.CancelTarget.performed += OnCancelTarget;
    _controls.CameraMovement.StartRotate.performed += OnCancelTarget;
    _controls.CameraMovement.Movement.performed += OnCancelTarget;
  }

  private void OnEnable()
  {
    _controls.Enable();
  }

  private void OnDisable()
  {
    _controls.Disable();
  }

  private void OnCancelTarget(InputAction.CallbackContext obj)
  {
    if (_target) _target.ShowStats = false;
    _target = null;
  }

  /// <summary>
  ///   Display stats of the clicked animal.
  /// </summary>
  /// <param name="_"></param>
  private void ClickedChar(InputAction.CallbackContext _)
  {
    RaycastHit hitTarget;
    var ray = mainCamera.ScreenPointToRay(GetMousePos());

    if (Physics.Raycast(ray, out hitTarget))
      if (hitTarget.transform.GetComponent<Animal>() is Animal animal)
      {
        if (_target) OnCancelTarget(_);
        animal.ShowStats = true;
        _target = animal;
      }
  }

  private Vector2 GetMousePos()
  {
    return Mouse.current.position.ReadValue();
  }
}