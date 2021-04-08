using System.Collections.Generic;
using UnityEngine;

namespace Animal.Sensor.SensorActions
{
  public sealed class SensorActionDelegate
  {
    private readonly IList<SensorAction> _sensorActions;

    public SensorActionDelegate(IList<SensorAction> sensorActions)
    {
      _sensorActions = sensorActions;
    }

    public void Do(Component component)
    {
      foreach (var sensedAction in _sensorActions)
      {
        var finalAction = sensedAction.Do(component);
        if (finalAction) return;
      }
    }
  }
}