using UnityEngine;
using UnityEngine.InputSystem;

public class DisplayCharacterStats : MonoBehaviour
{
  [SerializeField] private Camera mainCamera;
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
    DisplayStats(_target);

  }
  
  private Vector2 GetMousePos()
  {
    return Mouse.current.position.ReadValue();
  }

  /// <summary>
  /// Don't know what exactly what it should return yet, so I made it void. Also, method should perhaps take in an Animal, not a Transform?
  /// When clicking on a object, it returns "clicked".
  /// </summary>
  /// <param name="target"></param>
  private void DisplayStats(Transform target)
  {
    Debug.Log("clicked");
  }
}