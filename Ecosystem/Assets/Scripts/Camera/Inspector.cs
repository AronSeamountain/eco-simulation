using Core;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
  public sealed class Inspector : MonoBehaviour
  {
    [SerializeField] private EntityManager entityManager;
    [SerializeField] private UnityEngine.Camera mainCamera;
    [SerializeField] private PropertiesContainer propertiesContainer;
    private CameraControls _controls;
    private Selected _selected;

    private void Awake()
    {
      _controls = new CameraControls();
      _controls.CameraMovement.Selecting.performed += ClickedChar;
      _controls.CameraMovement.CancelTarget.performed += OnCancelTarget;
      _controls.CameraMovement.Rotate.performed += OnCancelTarget;
      _controls.CameraMovement.Movement.performed += OnCancelTarget;
    }

    private void Start()
    {
      ShowGlobalStats();
    }

    private void OnEnable()
    {
      _controls.Enable();
    }

    private void OnDisable()
    {
      _controls.Disable();
    }

    private void ShowGlobalStats()
    {
      propertiesContainer.ClearContent();
      propertiesContainer.Populate(entityManager.GetProperties());
    }

    private void OnCancelTarget(InputAction.CallbackContext obj)
    {
      var createWorldProperties = _selected != null;

      if (_selected != null)
        DeselectSelection();

      if (createWorldProperties)
        ShowGlobalStats();
    }

    /// <summary>
    ///   Removes the selected IInspectable and resets the properties card.
    /// </summary>
    private void DeselectSelection()
    {
      propertiesContainer.ClearContent();
      if (_selected != null && !_selected.Destroyed) _selected.Inspectable.ShowGizmos(false);
      _selected = null;
    }

    /// <summary>
    ///   Display stats of the clicked animal.
    /// </summary>
    /// <param name="_"></param>
    private void ClickedChar(InputAction.CallbackContext _)
    {
      var ray = mainCamera.ScreenPointToRay(GetMousePos());
      if (Physics.Raycast(ray, out var hitTarget))
      {
        var go = hitTarget.collider.gameObject;
        var inspectable = go.GetComponent<IInspectable>();
        if (inspectable == null) return;

        DeselectSelection();
        _selected = new Selected(go, inspectable);
        inspectable.ShowGizmos(true);
        propertiesContainer.Populate(inspectable.GetProperties());
      }
    }

    private Vector2 GetMousePos()
    {
      return Mouse.current.position.ReadValue();
    }

    private sealed class Selected
    {
      public Selected(GameObject gameObject, IInspectable inspectable)
      {
        _gameObject = gameObject;
        Inspectable = inspectable;
      }

      private GameObject _gameObject { get; }

      public IInspectable Inspectable { get; }

      public bool Destroyed => !_gameObject;
    }
  }
}