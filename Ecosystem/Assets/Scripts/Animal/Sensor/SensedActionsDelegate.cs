using System.Collections.Generic;
using UnityEngine;

namespace Animal.Sensor
{
  public sealed class SensedActionsDelegate
  {
    private IList<ObjectSensedAction> _sensedActions;

    public SensedActionsDelegate(IList<ObjectSensedAction> sensedActions)
    {
      _sensedActions = sensedActions;
    }

    public void Do(Component component)
    {
      foreach (var sensedAction in _sensedActions)
      {
        var finalAction = sensedAction.Do(component);
        if (finalAction) return;
      }
    }
  }
}