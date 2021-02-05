using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public sealed class CameraController : MonoBehaviour
{
  /// <summary>
  ///   The distance in the x-z plane to the target
  /// </summary>
  private const int Distance = 30;

  /// <summary>
  ///   The height we want the camera to be above the target
  /// </summary>
  private const int Height = 15;

  private const int RotationSpeed = 10;
  private const int ViewSpeed = 10;
  private const int Acceleration = 40;
  private const int FlyMovementSpeed = 10;
  [SerializeField] private Camera mainCamera;
  private Transform _cameraTransform;
  private CameraControls _controls;
  private float _followSpeed;
  private bool _rotate;
  private Transform _target;

  private void Awake()
  {
    _controls = new CameraControls();
    _controls.CameraMovement.Selecting.performed += OnSelect;
    _controls.CameraMovement.CancelTarget.performed += OnCancelTarget;
    _controls.CameraMovement.StartRotate.performed += OnStartRotate;
    _controls.CameraMovement.EndRotate.performed += OnEndRotate;
    _controls.CameraMovement.Movement.performed += OnMovement;
  }

  private void Start()
  {
    _cameraTransform = transform;
  }

  private void Update()
  {
    Move();
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

  private void OnMovement(InputAction.CallbackContext _)
  {
    _target = null;
    _followSpeed = 0;
  }

  private void OnStartRotate(InputAction.CallbackContext _)
  {
    _target = null;
    _followSpeed = 0;
    _rotate = true;
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
  }

  private void Rotate()
  {
    if (!_rotate) return;

    var deltaMouse = _controls.CameraMovement.View.ReadValue<Vector2>();
    deltaMouse *= -1; // Flip

    _cameraTransform.Rotate(
      new Vector3(1, 0, 0), deltaMouse.y * ViewSpeed * Time.deltaTime
    );

    _cameraTransform.Rotate(
      new Vector3(0, 1, 0), -deltaMouse.x * ViewSpeed * Time.deltaTime, Space.World
    );
  }

  private void OnEndRotate(InputAction.CallbackContext _)
  {
    _rotate = false;
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
  }

  private void OnCancelTarget(InputAction.CallbackContext _)
  {
    _target = null;
    _followSpeed = 0;
  }

  private void OnSelect(InputAction.CallbackContext _)
  {
    RaycastHit hitTarget;
    var ray = mainCamera.ScreenPointToRay(GetMousePos());

    if (Physics.Raycast(ray, out hitTarget))
      _target = hitTarget.transform;
  }

  private Vector2 GetMousePos()
  {
    return Mouse.current.position.ReadValue();
  }

  private void LookAt()
  {
    if (_target == null) return;
    var dirToObj = (_target.position - _cameraTransform.position).normalized;
    var desiredRotation = Quaternion.LookRotation(dirToObj, Vector3.up);
    _cameraTransform.rotation =
      Quaternion.Slerp(_cameraTransform.rotation, desiredRotation, RotationSpeed * Time.deltaTime);
  }

  /// <summary>
  ///   Camera movement using WASD or arrow keys. Travel up and down the y-axis using Z and X.
  /// </summary>
  private void Move()
  {
    if (_target) return;

    var input = _controls.CameraMovement.Movement.ReadValue<Vector2>();
    var direction = new Vector3();
    direction += input.x * _cameraTransform.right;
    direction += input.y * _cameraTransform.forward;
    _cameraTransform.position += direction * (FlyMovementSpeed * Time.deltaTime);
  }

  /// <summary>
  ///   Camera zooms in on a clicked object
  /// </summary>
  private void Follow()
  {
    if (!_target) return;

    var targetFront = _target.forward;
    var desiredPosition = _target.position - targetFront * Distance + new Vector3(0, Height);

    var hasArrived = Vector3Util.InRange(desiredPosition, _cameraTransform.position, 5f);
    if (hasArrived)
    {
      _followSpeed = 0;
    }
    else
    {
      _followSpeed += Acceleration * Time.deltaTime;
      var direction = (desiredPosition - _cameraTransform.position).normalized;
      _cameraTransform.position += direction * (_followSpeed * Time.deltaTime);
    }
  }
}