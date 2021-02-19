﻿using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisplayCharacterStats : MonoBehaviour
{
  [SerializeField] private Camera mainCamera;
  [SerializeField] private CardFactory cardFactory;
  private CameraControls _controls;
  private Animal _target;
  private IStatable _targetIS;

  private void Awake()
  {
    _controls = new CameraControls();
    _controls.CameraMovement.Selecting.performed += ClickedChar;
    _controls.CameraMovement.CancelTarget.performed += OnCancelTarget;
    _controls.CameraMovement.Rotate.performed += OnCancelTarget;
    _controls.CameraMovement.Movement.performed += OnCancelTarget;
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
    if (_targetIS != null) cardFactory.ClearContent();
    _targetIS = null;
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
      var interfaces = hitTarget.collider.gameObject.GetComponents<MonoBehaviour>();
      foreach (var mb in interfaces)
        if (mb is IStatable statable)
        {
          if (_targetIS != null) OnCancelTarget(_);
          _targetIS = statable;
          cardFactory.Populate(_targetIS.GetStats());
        }
    }
  }

  private Vector2 GetMousePos()
  {
    return Mouse.current.position.ReadValue();
  }
}