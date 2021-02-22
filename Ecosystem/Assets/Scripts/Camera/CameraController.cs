using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Camera
{
  public sealed class CameraController : MonoBehaviour
  {
    /// <summary>
    ///   The distance in the x-z plane to the target
    /// </summary>
    private const int DistanceBehind = 20;

    /// <summary>
    ///   The height we want the camera to be above the target
    /// </summary>
    private const int DistanceOver = 10;

    private const int RotationSpeed = 10;
    private const int ViewSpeed = 10;
    private const int FreeMovementStandardSpeed = 10;
    private const int FreeMovementFastSpeed = 30;
    private const int FixedTargetSpeed = 1;
    [SerializeField] private UnityEngine.Camera mainCamera;
    [SerializeField] private LayerMask selectableLayers;
    private Transform _camTransform;
    private CameraControls _controls;
    private bool _moveFast;
    private bool _rotate;
    private Transform _target;
    private int FreeMovementSpeed => _moveFast ? FreeMovementFastSpeed : FreeMovementStandardSpeed;

    private Transform Target
    {
      get => _target;
      set
      {
        if (_target == value) return;

        // Remove old outline from current target
        if (_target && _target.GetComponent<Outline>() is Outline oldOutline)
          Destroy(oldOutline);

        // Set new target
        _target = value ? value.root : null;

        // Add outline to new target
        if (Target)
        {
          var hasOutline = Target.GetComponent<Outline>() != null;
          if (hasOutline) return;

          var outline = Target.gameObject.AddComponent<Outline>();
          outline.OutlineWidth = 7f;
        }
      }
    }

    private void Awake()
    {
      _controls = new CameraControls();
      _controls.CameraMovement.Selecting.performed += OnSelect;
      _controls.CameraMovement.CancelTarget.performed += OnCancelTarget;
      _controls.CameraMovement.Rotate.started += OnRotate;
      _controls.CameraMovement.Rotate.canceled += OnRotate;
      _controls.CameraMovement.Movement.performed += OnMovement;
      _controls.CameraMovement.MoveFast.started += OnMoveFast;
      _controls.CameraMovement.MoveFast.canceled += OnMoveFast;
    }

    private void Start()
    {
      _camTransform = transform;
    }

    private void Update()
    {
      FreeMovement();
      Follow();
      LookAt();
      Rotate();
    }

    private void OnEnable()
    {
      _controls.Enable();
    }

    private void OnDisable()
    {
      _controls.Disable();
    }

    private void OnMoveFast(InputAction.CallbackContext context)
    {
      if (context.started)
        _moveFast = true;
      else if (context.canceled)
        _moveFast = false;
    }

    private void OnMovement(InputAction.CallbackContext _)
    {
      Target = null;
    }

    private void OnRotate(InputAction.CallbackContext context)
    {
      if (context.started)
      {
        _rotate = true;
        Target = null;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
      }
      else if (context.canceled)
      {
        _rotate = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
      }
    }

    private void Rotate()
    {
      if (!_rotate) return;

      var deltaMouse = _controls.CameraMovement.View.ReadValue<Vector2>();
      deltaMouse *= -1; // Flip

      _camTransform.Rotate(
        new Vector3(1, 0, 0), deltaMouse.y * ViewSpeed * Time.deltaTime
      );

      _camTransform.Rotate(
        new Vector3(0, 1, 0), -deltaMouse.x * ViewSpeed * Time.deltaTime, Space.World
      );
    }

    private void OnCancelTarget(InputAction.CallbackContext _)
    {
      Target = null;
    }

    private void OnSelect(InputAction.CallbackContext _)
    {
      var ray = mainCamera.ScreenPointToRay(GetMousePos());

      if (Physics.Raycast(ray, out var hitTarget))
        if (selectableLayers.Contains(hitTarget.collider.gameObject.layer))
          Target = hitTarget.transform;
    }

    private Vector2 GetMousePos()
    {
      return Mouse.current.position.ReadValue();
    }

    private void LookAt()
    {
      if (!Target) return;
      var dirToObj = (Target.position - _camTransform.position).normalized;
      var desiredRotation = Quaternion.LookRotation(dirToObj, Vector3.up);
      _camTransform.rotation =
        Quaternion.Slerp(_camTransform.rotation, desiredRotation, RotationSpeed * Time.deltaTime);
    }

    /// <summary>
    ///   Camera movement using WASD or arrow keys. Travel up and down the y-axis using Z and X.
    /// </summary>
    private void FreeMovement()
    {
      if (Target) return;

      var input = _controls.CameraMovement.Movement.ReadValue<Vector2>();
      var direction = new Vector3();
      direction += input.x * _camTransform.right;
      direction += input.y * _camTransform.forward;
      _camTransform.position += direction * (FreeMovementSpeed * Time.deltaTime);
    }

    /// <summary>
    ///   Camera zooms in on a clicked object
    /// </summary>
    private void Follow()
    {
      if (!Target) return;
      var desiredPos = Target.position - Target.forward * DistanceBehind + new Vector3(0, DistanceOver);
      _camTransform.position = Vector3.Lerp(_camTransform.position, desiredPos, FixedTargetSpeed * Time.deltaTime);
    }
  }
}