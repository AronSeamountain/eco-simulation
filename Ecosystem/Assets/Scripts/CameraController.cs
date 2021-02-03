﻿using UnityEngine;
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

  private const float RotationSpeed = 10;
  private const float PanningSpeed = 100;
  private const int MovementSpeed = 10;
  [SerializeField] private Camera mainCamera;
  private Transform _cameraTransform;
  private CameraControls _controls;
  private Vector3 _previousMousePos;
  private bool _rotate;
  private Transform _target;


  private void Awake()
  {
    _controls = new CameraControls();
    _controls.CameraMovement.Selecting.performed += OnSelect;
    _controls.CameraMovement.CancelTarget.performed += OnCancelTarget;
    _controls.CameraMovement.StartRotate.performed += OnStartRotate;
    _controls.CameraMovement.EndRotate.performed += OnEndRotate;
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

  private void OnStartRotate(InputAction.CallbackContext context)
  {
    _rotate = true;
    _previousMousePos = mainCamera.ScreenToViewportPoint(GetMousePos());
  }

  private void Rotate()
  {
    if (_rotate)
    {
      var mousePos = GetMousePos();
      var mouseDirection = _previousMousePos - mainCamera.ScreenToViewportPoint(mousePos);

      _cameraTransform.Rotate(new Vector3(1, 0, 0), mouseDirection.y * 180 * PanningSpeed * Time.deltaTime);
      _cameraTransform.Rotate(new Vector3(0, 1, 0), -mouseDirection.x * 180 * PanningSpeed * Time.deltaTime,
        Space.World);

      _previousMousePos = mainCamera.ScreenToViewportPoint(mousePos);
    }
  }

  private void OnEndRotate(InputAction.CallbackContext obj)
  {
    _rotate = false;
  }


  private void OnCancelTarget(InputAction.CallbackContext obj)
  {
    _target = null;
  }

  private void OnSelect(InputAction.CallbackContext context)
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
    if (_target)
      return;

    var input = _controls.CameraMovement.Movement.ReadValue<Vector2>();
    var direction = new Vector3();
    direction += input.x * _cameraTransform.right;
    direction += input.y * _cameraTransform.forward;
    _cameraTransform.position += direction * (MovementSpeed * Time.deltaTime);
  }


  /// <summary>
  ///   Camera zooms in on a clicked object
  /// </summary>
  private void Follow()
  {
    if (!_target) return;

    var targetFront = _target.forward;
    var desiredPosition = _target.position - targetFront * Distance + new Vector3(0, Height);

    var hasArrived = (desiredPosition - _cameraTransform.position).magnitude <= 0.1f;
    if (!hasArrived)
    {
      var direction = (desiredPosition - _cameraTransform.position).normalized;
      _cameraTransform.position += direction * (MovementSpeed * Time.deltaTime);
    }
  }
}