using UnityEngine;
using UnityEngine.InputSystem;

public class DisplayCharacterStats : MonoBehaviour
{
  [SerializeField] private Camera mainCamera;
  private StatsCardUi _card;
  private CameraControls _controls;

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
    {
      if (hitTarget.transform.GetComponent<Animal>() is Animal animal)
      {
        animal.ShowStats = true;
      }
    }
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