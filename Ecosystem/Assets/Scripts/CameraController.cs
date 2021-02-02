using UnityEngine;

public class CameraController : MonoBehaviour
{
  [SerializeField] private Camera mainCamera;
  [SerializeField] private CharacterController cameraController;
  [SerializeField] private int movementSpeed;

  [SerializeField] private Transform target;

  private Transform _cameraTransform;
  private Vector3 _targetPosition;

  /// The distance in the x-z plane to the target
  private readonly float _distance = 10.0f;

  /// The height we want the camera to be above the target
  private readonly float _height = 5.0f;

  private readonly float _heightDamping = 1;

  private Vector3 _previousMousePos;

  private readonly float _rotationDamping = 1;
  private Vector3 _target;
  private bool _yToggle = true;

  private void Start()
  {
    _cameraTransform = mainCamera.transform;
    _targetPosition = target.position;
  }

  private void Update()
  {
    OrdinaryMovement();
    RotationalMovement();
    ClickObject();
  }

  private void OrdinaryMovement()
  {
    var direction = new Vector3(0, 0, 0);

    //WASD-movement as well as arrows
    if (Input.GetKey("w") || Input.GetKey("up")) direction += _cameraTransform.forward;

    if (Input.GetKey("s") || Input.GetKey("down")) direction -= _cameraTransform.forward;

    if (Input.GetKey("a") || Input.GetKey("left")) direction -= _cameraTransform.right;

    if (Input.GetKey("d") || Input.GetKey("right")) direction += _cameraTransform.right;

    //travel up and down the y-axis using space, press left shift once to change direction.

    if (Input.GetKeyDown("left shift")) _yToggle = !_yToggle;
    if (Input.GetKey("space"))
    {
      if (_yToggle)
        direction.y += 1;
      else
        direction.y -= 1;
    }

    direction.Normalize();
    _cameraTransform.position += direction * (movementSpeed * Time.deltaTime);
  }

  private void RotationalMovement()
  {
    //rotational movement using right mouse button
    if (Input.GetMouseButtonDown(1)) _previousMousePos = mainCamera.ScreenToViewportPoint(Input.mousePosition);

    if (Input.GetMouseButton(1))
    {
      var mouseDirection = _previousMousePos - mainCamera.ScreenToViewportPoint(Input.mousePosition);

      _cameraTransform.Rotate(new Vector3(1, 0, 0), mouseDirection.y * 180);
      _cameraTransform.Rotate(new Vector3(0, 1, 0), -mouseDirection.x * 180, Space.World);

      _previousMousePos = mainCamera.ScreenToViewportPoint(Input.mousePosition);
    }
  }

  /// <summary>
  ///   Camera zooms in on a clicked object.
  /// </summary>
  private void ClickObject()
  {
    if (Input.GetMouseButtonDown(0))
    {
      var hitTarget = new RaycastHit();
      var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out hitTarget))
      {
        target = hitTarget.transform;
        if (target.parent != null) target = target.parent;
      }
    }

    var hasArrived = (_targetPosition - _cameraTransform.position).magnitude < _distance + 1;
    if (!hasArrived)
    {
      var direction = (_targetPosition - _cameraTransform.position).normalized;
      cameraController.Move(direction * (movementSpeed * Time.deltaTime));
      _cameraTransform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
    else
    {
      if (!target)
        return;

      // Calculate the current rotation angles
      var wantedRotationAngle = target.eulerAngles.y;
      var wantedHeight = _targetPosition.y + _height;

      var currentRotationAngle = _cameraTransform.eulerAngles.y;
      var currentHeight = _cameraTransform.position.y;

      // Damp the rotation around the y-axis
      currentRotationAngle =
        Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, _rotationDamping * Time.deltaTime);

      // Damp the height
      currentHeight = Mathf.Lerp(currentHeight, wantedHeight, _heightDamping * Time.deltaTime);

      // Convert the angle into a rotation
      var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

      // Set the position of the camera on the x-z plane to:
      // distance meters behind the target
      _cameraTransform.position = _targetPosition;
      _cameraTransform.position -= currentRotation * Vector3.forward * _distance;


      // Set the height of the camera
      _cameraTransform.position = new Vector3(_cameraTransform.position.x, currentHeight, _cameraTransform.position.z);

      // Always look at the target
      _cameraTransform.LookAt(target);
    }

    if (Input.GetKeyDown(KeyCode.Escape))
    {
      mainCamera.Reset();
      target = null;
    }
  }
}