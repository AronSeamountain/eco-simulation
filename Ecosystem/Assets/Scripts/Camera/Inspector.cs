using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
  public sealed class Inspector : MonoBehaviour
  {
    [SerializeField] private UnityEngine.Camera mainCamera;
    [SerializeField] private PropertiesContainer propertiesContainer;
    private CameraControls _controls;
    private IInspectable _lastSelected;

    private void Awake()
    {
      _controls = new CameraControls();
      _controls.CameraMovement.Selecting.performed += ClickedChar;
      _controls.CameraMovement.CancelTarget.performed += OnCancelTarget;
      _controls.CameraMovement.Rotate.performed += OnCancelTarget;
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
      ResetInspectStats();
    }

    private void ResetInspectStats()
    {
      propertiesContainer.ClearContent();

      if (_lastSelected != null)
      {
        _lastSelected.GetStats(false);
        _lastSelected = null;
      }
    }

    /// <summary>
    ///   Display stats of the clicked animal.
    /// </summary>
    /// <param name="_"></param>
    private void ClickedChar(InputAction.CallbackContext _)
    {
      var ray = mainCamera.ScreenPointToRay(GetMousePos());
      if (Physics.Raycast(ray, out var hitTarget))
      {
        var inspectable = hitTarget.collider.gameObject.GetComponent<IInspectable>();
        if (inspectable == null) return;

        ResetInspectStats();
        _lastSelected = inspectable;
        propertiesContainer.Populate(inspectable.GetStats(true));
      }
    }

    private Vector2 GetMousePos()
    {
      return Mouse.current.position.ReadValue();
    }
  }
}