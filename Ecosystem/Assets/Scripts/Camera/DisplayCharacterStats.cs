using System;
using Core;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
  public class DisplayCharacterStats : MonoBehaviour
  {
    [SerializeField] private UnityEngine.Camera mainCamera;
    [SerializeField] private PropertiesCard propertiesCard;
    [SerializeField] private EntityManager entityManager;
    private CameraControls _controls;
    private IStatable _targetIS;

    private void Awake()
    {
      _controls = new CameraControls();
      _controls.CameraMovement.Selecting.performed += ClickedChar;
      _controls.CameraMovement.CancelTarget.performed += ShowGlobalStats;
      _controls.CameraMovement.Rotate.performed += ShowGlobalStats;
      _controls.CameraMovement.Movement.performed += ShowGlobalStats;
    }

    private void Start()
    {
      entityManager.EcoSystemStatsChangedListeners += AnimalCountChanged;
    }

    private void AnimalCountChanged()
    {
      if (_targetIS == null || _targetIS == entityManager)
      {
        ShowGlobalStats(new InputAction.CallbackContext());
      }
    }
    private void OnEnable()
    {
      _controls.Enable();
    }

    private void OnDisable()
    {
      _controls.Disable();
    }

    private void ShowGlobalStats(InputAction.CallbackContext _)
    {
      OnCancelTarget(_);
      DisplayStat(entityManager);
    }
    private void OnCancelTarget(InputAction.CallbackContext _)
    {
      if (_targetIS != null)
        _targetIS.GetStats(false);
      propertiesCard.ClearContent();
      _targetIS = null;
    }

    private void DisplayStat(IStatable statable)
    {
      _targetIS = statable;
      propertiesCard.Populate(statable.GetStats(true));
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
        if (_targetIS != null)
        {
          OnCancelTarget(_);
        }
        var statable = hitTarget.collider.gameObject.GetComponent<IStatable>();
        if (statable == null) statable = entityManager;
       
        DisplayStat(statable);
      }
    }

    private Vector2 GetMousePos()
    {
      return Mouse.current.position.ReadValue();
    }
  }
}