using System;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utils;
using Random = UnityEngine.Random;

namespace Camera
{
  public sealed class CameraController : MonoBehaviour
  {
    private const int RotationSpeed = 10;
    private const float CameraLookAcceleration = 1.5f;
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

    public Vector2 LookVelocity
    {
      get => _lookVelocity;
      private set
      {
        const float maxLookSpeed = 100f;

        var x = Mathf.Clamp(value.x, -maxLookSpeed, maxLookSpeed);
        var y = Mathf.Clamp(value.y, -maxLookSpeed, maxLookSpeed);

        _lookVelocity = new Vector2(x, y);
      }
    }

    private const float CameraLookFriction = 20f;

    /// <summary>
    ///   The distance in the x-z plane to the target
    /// </summary>
    private int DistanceBehind = 5;

    /// <summary>
    ///   The height we want the camera to be above the target
    /// </summary>
    private int DistanceOver = 5;

    private Vector2 _lookVelocity;

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
      _controls.CameraMovement.ZoomIn.performed += OnZoomIn;
      _controls.CameraMovement.ZoomOut.performed += OnZoomOut;
      _controls.CameraMovement.Restart.performed += OnRestart;
      _controls.CameraMovement.IncreaseFov.performed += OnIncreaseFov;
      _controls.CameraMovement.DecreaseFov.performed += OnDecreaseFov;
    }

    private static void OnRestart(InputAction.CallbackContext obj)
    {
      SceneManager.LoadScene("Menu");
      EntityManager.Restart();
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

    private void OnIncreaseFov(InputAction.CallbackContext _)
    {
      ChangeFov(true);
    }

    private void OnDecreaseFov(InputAction.CallbackContext _)
    {
      ChangeFov(false);
    }

    private void OnZoomIn(InputAction.CallbackContext _)
    {
      ChangeZoom(true);
    }

    private void OnZoomOut(InputAction.CallbackContext _)
    {
      ChangeZoom(false);
    }

    private void ChangeFov(bool increase)
    {
      var delta = increase ? -5 : 5;
      mainCamera.fieldOfView += delta;
    }

    private void ChangeZoom(bool increase)
    {
      var delta = increase ? -5 : 5;
      DistanceBehind += delta;
      DistanceOver += delta;

      DistanceBehind = Mathf.Clamp(DistanceBehind, 0, 10000);
      DistanceOver = Mathf.Clamp(DistanceOver, 0, 10000);
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
        LookVelocity = new Vector3();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
      }
    }

    private void Rotate()
    {
      if (!_rotate) return;

      var deltaMouse = _controls.CameraMovement.View.ReadValue<Vector2>();
      deltaMouse *= -1; // Flip

      deltaMouse = new Vector2(
        Mathf.Clamp(deltaMouse.x, -1, 1),
        Mathf.Clamp(deltaMouse.y, -1, 1)
      );

      var oldV = LookVelocity;
      LookVelocity += deltaMouse;
      var newV = LookVelocity;

      var lerp = Vector2.Lerp(oldV, newV, 4f);

      _camTransform.Rotate(
        new Vector3(1, 0, 0), lerp.y * CameraLookAcceleration * Time.deltaTime
      );

      _camTransform.Rotate(
        new Vector3(0, 1, 0), -lerp.x * CameraLookAcceleration * Time.deltaTime, Space.World
      );

      if (LookVelocity.x < 0)
      {
        LookVelocity += new Vector2(CameraLookFriction, 0) * Time.deltaTime;
      }
      else
      {
        LookVelocity -= new Vector2(CameraLookFriction, 0) * Time.deltaTime;
      }

      if (LookVelocity.y < 0)
      {
        LookVelocity += new Vector2(0, CameraLookFriction) * Time.deltaTime;
      }
      else
      {
        LookVelocity -= new Vector2(0, CameraLookFriction) * Time.deltaTime;
      }
    }

    private void OnCancelTarget(InputAction.CallbackContext _)
    {
      Target = null;
    }

    private void OnSelect(InputAction.CallbackContext _)
    {
      var ray = mainCamera.ScreenPointToRay(GetMousePos());

      if (Physics.Raycast(ray, out var hitTarget, 1000, RayCastUtil.CastableLayers))
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