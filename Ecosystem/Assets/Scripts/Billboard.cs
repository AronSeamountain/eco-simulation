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
    //transform.LookAt(_cameraTransform);
    transform.rotation = _cameraTransform.rotation;
  }
}