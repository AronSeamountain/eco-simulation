using UnityEngine;
using UnityEngine.InputSystem;

public class DisplayCharacterStats : MonoBehaviour
{
  [SerializeField] private Camera mainCamera;
  private CameraControls _controls;

  private void Awake()
  {
    _controls = new CameraControls();
    _controls.CameraMovement.Selecting.performed += ClickedChar;
  }

  private void OnEnable()
  {
    _controls.Enable();
  }

  private void OnDisable()
  {
    _controls.Disable();
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
        animal.ShowStats = true;
  }

  private Vector2 GetMousePos()
  {
    return Mouse.current.position.ReadValue();
  }
}