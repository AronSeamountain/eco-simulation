using System;
using UnityEngine;

public sealed class Billboard : MonoBehaviour
{
  private Transform _cameraTransform;

  private void Start()
  {
    var mainCamera = Camera.main;
    if (mainCamera == null) throw new Exception("No main camera is set.");
    _cameraTransform = mainCamera.transform;
  }

  private void Update()
  {
    // TODO: Dont update if is not shown
    transform.LookAt(_cameraTransform);
  }
}