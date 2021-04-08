using System.Collections.Generic;
using UnityEngine;

namespace Animal.Sensor.SensorActions
{
  public sealed class SensorActionDelegate
  {
    private readonly IList<SensorAction> _sensorActions;

    public SensorActionDelegate()
    {
      _sensorActions = new List<SensorAction>();
    }

    public void AddAction(SensorAction sensorAction)
    {
      _sensorActions.Add(sensorAction);
    }

    public void Do(Component component)
    {
      foreach (var action in _sensorActions)
      {
        var finalAction = action.Do(component);
        if (finalAction) return;
      }
    }
  }
}