using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
  public class DisplayCharacterStats : MonoBehaviour
  {
    [SerializeField] private UnityEngine.Camera mainCamera;
    [SerializeField] private PropertiesCard propertiesCard;
    private CameraControls _controls;
    private IStatable _targetIS;

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
      if (_targetIS != null)
        _targetIS.GetStats(false);
      propertiesCard.ClearContent();
      _targetIS = null;
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
        if (_targetIS != null) OnCancelTarget(_);
        var statable = hitTarget.collider.gameObject.GetComponent<IStatable>();
        if (statable == null) return;
        _targetIS = statable;
        propertiesCard.Populate(statable.GetStats(true));
      }
    }

    private Vector2 GetMousePos()
    {
      return Mouse.current.position.ReadValue();
    }
  }
}