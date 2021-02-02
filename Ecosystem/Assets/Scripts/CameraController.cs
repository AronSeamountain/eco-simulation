using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  [SerializeField] private Camera camera;
  [SerializeField] private CharacterController cameraController;
  [SerializeField] private int movementSpeed;

  [SerializeField] private Transform target;

  private Transform _cameraTransform;

  // The distance in the x-z plane to the target
  private float _distance = 10.0f;

  // the height we want the camera to be above the target
  private float _height = 5.0f;

  private float _rotationDamping = 1;

  private float _heightDamping = 1;

  private Vector3 _previousMousePos;
  private Vector3 _target;

  private void Start()
  {
    _cameraTransform = camera.transform;
  }


  // Update is called once per frame
  private void Update()
  {
    OrdinaryMovement();
    RotationalMovement();
    ClickObject();
  }

  /// <summary>
  /// Camera movement using WASD or arrow keys. Travel up and down the y-axis using Z and X.
  /// </summary>
  private void OrdinaryMovement()
  {
    var direction = new Vector3(0, 0, 0);

    if (Input.GetButton("Horizontal"))
    {
      direction += Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed * _cameraTransform.right ;
    }

    if (Input.GetButton("Vertical"))
    {
      direction += Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed * _cameraTransform.forward;
    }

    if (Input.GetButton("CameraY"))
    {
      direction.y += Input.GetAxis("CameraY") * Time.deltaTime * 1;
    }
    
    direction.Normalize();
    _cameraTransform.position += direction * (movementSpeed * Time.deltaTime);
  }

  private void RotationalMovement()
  {
    //rotational movement using right mouse button
    if (Input.GetMouseButtonDown(1)) _previousMousePos = camera.ScreenToViewportPoint(Input.mousePosition);

    if (Input.GetMouseButton(1))
    {
      var mouseDirection = _previousMousePos - camera.ScreenToViewportPoint(Input.mousePosition);

      _cameraTransform.Rotate(new Vector3(1, 0, 0), mouseDirection.y * 180);
      _cameraTransform.Rotate(new Vector3(0, 1, 0), -mouseDirection.x * 180, Space.World);

      _previousMousePos = camera.ScreenToViewportPoint(Input.mousePosition);
    }
  }
  
  /// <summary>
  /// Camera zooms in on a clicked object
  /// </summary>
  private void ClickObject()
  {
    if (Input.GetButton("Fire1"))
    {
      var hitTarget = new RaycastHit();
      var ray = camera.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out hitTarget))
      {
        target = hitTarget.transform;
        if (target.parent != null) target = target.parent;
      }
    }

    var hasArrived = (target.position - transform.position).magnitude < _distance + 1;
    if (!hasArrived)
    {
      var direction = (target.position - transform.position).normalized;
      cameraController.Move(direction * (movementSpeed * Time.deltaTime));
      transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
    else
    {
      if (!target)
        return;

      // Calculate the current rotation angles
      var wantedRotationAngle = target.eulerAngles.y;
      var wantedHeight = target.position.y + _height;

      var currentRotationAngle = transform.eulerAngles.y;
      var currentHeight = transform.position.y;

      // Damp the rotation around the y-axis
      currentRotationAngle =
        Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, _rotationDamping * Time.deltaTime);

      // Damp the height
      currentHeight = Mathf.Lerp(currentHeight, wantedHeight, _heightDamping * Time.deltaTime);

      // Convert the angle into a rotation
      var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

      // Set the position of the camera on the x-z plane to:
      // distance meters behind the target
      transform.position = target.position;
      transform.position -= currentRotation * Vector3.forward * _distance;


      // Set the height of the camera
      transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

      // Always look at the target
      transform.LookAt(target);
    }

    if (Input.GetButton("Cancel"))
    {
      camera.Reset();
      target = null;
    }
  }
}