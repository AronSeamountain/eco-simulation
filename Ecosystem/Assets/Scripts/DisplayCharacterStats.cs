using UnityEngine;
using UnityEngine.InputSystem;

public class DisplayCharacterStats : MonoBehaviour
{
  [SerializeField] private Camera mainCamera;
  [SerializeField] private Animal animal;
  private StatsCardUi _card;
  private CameraControls _controls;
  private Transform _target;

  private void Awake()
  {
    _controls = new CameraControls();
    _controls.CameraMovement.Selecting.performed += ClickedChar;
  }

  // Update is called once per frame
  private void Update()
  {
  }

  private void OnEnable()
  {
    _controls.Enable();
  }

  private void OnDisable()
  {
    _controls.Disable();
  }

  private void ClickedChar(InputAction.CallbackContext _)
  {
    RaycastHit hitTarget;
    var ray = mainCamera.ScreenPointToRay(GetMousePos());

    if (Physics.Raycast(ray, out hitTarget))
      _target = hitTarget.transform;
    //if (_target != animal.transform) return; // does not work
    DisplayStats(_target);
  }

  private Vector2 GetMousePos()
  {
    return Mouse.current.position.ReadValue();
  }

  /// <summary>
  ///  Display stats of clicked animal.
  /// </summary>
  /// <param name="target"></param>
  private void DisplayStats(Transform target)
  {
    Debug.Log("clicked");
  }
}