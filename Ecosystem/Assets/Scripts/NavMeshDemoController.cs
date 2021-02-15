using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public sealed class NavMeshDemoController : MonoBehaviour
{
  [SerializeField] private NavMeshAgent agent;
  [SerializeField] private Camera camera;

  private void Update()
  {
    var mousePos = Mouse.current.position.ReadValue();

    var ray = camera.ScreenPointToRay(mousePos);

    if (Physics.Raycast(ray, out var hit))
    {
      agent.SetDestination(hit.point);
    }
  }
}
