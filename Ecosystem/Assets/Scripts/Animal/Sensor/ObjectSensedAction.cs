using UnityEngine;

namespace Animal.Sensor
{
  public sealed class ObjectSensedAction
  {
    public delegate bool SensedAction(Component component);

    private readonly SensedAction sensedAction;

    public ObjectSensedAction(SensedAction sensedAction)
    {
      this.sensedAction = sensedAction;
    }

    /// <summary>
    ///   Performs specific behaviour for a certain object.
    /// </summary>
    /// <param name="component">The component which was collided with.</param>
    /// <returns>True if the action is a "final" action, the rest of the actions should not be looped trough.</returns>
    public bool Do(Component component)
    {
      return sensedAction(component);
    }
  }
}