using UnityEngine;
using UnityEngine.InputSystem;

public sealed class CameraController : MonoBehaviour
{
  /// <summary>
  ///   The distance in the x-z plane to the target
  /// </summary>
  private const float Distance = 10.0f;

  /// <summary>
  ///   The height we want the camera to be above the target
  /// </summary>
  private const float Height = 5.0f;

  private const float RotationDamping = 1;
  private const float HeightDamping = 1;
  private const float RotationSpeed = 10;
  [SerializeField] private Camera camera;
  [SerializeField] private CharacterController cameraController;
  [SerializeField] private int movementSpeed;
  [SerializeField] private Transform target;
  private Transform _cameraTransform;
  private CameraControls _controls;
  private Vector3 _previousMousePos;
  private Vector3 _target;

  private void Awake()
  {
    _controls = new CameraControls();
    _controls.FreeMovement.Selecting.performed += OnSelect;
    _controls.FreeMovement.CancelTarget.performed += OnCancelTarget;
  }

  private void Start()
  {
    _cameraTransform = transform;
  }

  // Update is called once per frame
  private void Update()
  {
    Move();
    Rotate();
    Follow();
    LookAt();
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
    target = null;
  }

  private void OnSelect(InputAction.CallbackContext context)
  {
    RaycastHit hitTarget;
    var ray = camera.ScreenPointToRay(GetMousePos());

    if (Physics.Raycast(ray, out hitTarget))
      target = hitTarget.transform;
  }

  private Vector2 GetMousePos()
  {
    return Mouse.current.position.ReadValue();
  }

  private void LookAt()
  {
    if (target == null) return;
    var dirToObj = (target.position - transform.position).normalized;
    var desiredRotation = Quaternion.LookRotation(dirToObj, Vector3.up);
    transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, RotationSpeed * Time.deltaTime);
  }

  /// <summary>
  ///   Camera movement using WASD or arrow keys. Travel up and down the y-axis using Z and X.
  /// </summary>
  private void Move()
  {
    var input = _controls.FreeMovement.Movement.ReadValue<Vector2>();
    var direction = new Vector3();
    direction += input.x * _cameraTransform.right;
    direction += input.y * _cameraTransform.forward;
    _cameraTransform.position += direction * (movementSpeed * Time.deltaTime);
  }

  private void Rotate()
  {
    //rotational movement using right mouse button
    if (false) _previousMousePos = camera.ScreenToViewportPoint(new Vector3());

    if (false)
    {
      var mousePos = GetMousePos();
      var mouseDirection = _previousMousePos - camera.ScreenToViewportPoint(mousePos);

      _cameraTransform.Rotate(new Vector3(1, 0, 0), mouseDirection.y * 180);
      _cameraTransform.Rotate(new Vector3(0, 1, 0), -mouseDirection.x * 180, Space.World);

      _previousMousePos = camera.ScreenToViewportPoint(mousePos);
    }
  }

  /// <summary>
  ///   Camera zooms in on a clicked object
  /// </summary>
  private void Follow()
  {
    if (!target) return;

    var targetFront = target.forward;
    var d1 = (target.position - _cameraTransform.position).normalized * Distance;
    var d2 = target.position - targetFront * Distance + new Vector3(0, Height);
    var desiredPosition = d2;

    var hasArrived = (desiredPosition - _cameraTransform.position).magnitude <= 0.1f;
    if (!hasArrived)
    {
      var direction = (desiredPosition - _cameraTransform.position).normalized;
      _cameraTransform.position += direction * (movementSpeed * Time.deltaTime);
    }

    return;
    {
      return;
      // Calculate the current rotation angles
      var wantedRotationAngle = target.eulerAngles.y;
      var wantedHeight = target.position.y + Height;

      var currentRotationAngle = transform.eulerAngles.y;
      var currentHeight = transform.position.y;

      // Damp the rotation around the y-axis
      currentRotationAngle =
        Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, RotationDamping * Time.deltaTime);

      // Damp the height
      currentHeight = Mathf.Lerp(currentHeight, wantedHeight, HeightDamping * Time.deltaTime);

      // Convert the angle into a rotation
      var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

      // Set the position of the camera on the x-z plane to:
      // distance meters behind the target
      transform.position = target.position;
      transform.position -= currentRotation * Vector3.forward * Distance;

      // Set the height of the camera
      transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

      // Always look at the target
      transform.LookAt(target);
    }
  }
}