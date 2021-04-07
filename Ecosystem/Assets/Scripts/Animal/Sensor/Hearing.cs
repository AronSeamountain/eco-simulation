using System.Collections.Generic;
using UnityEngine;

namespace Animal.Sensor
{
  /// <summary>
  ///   Triggers on sounds from other animals
  /// </summary>
  public sealed class Hearing : MonoBehaviour
  {
    /// <summary>
    ///   Is invoked when another animal is heard.
    /// </summary>
    /// <param name="animal"></param>
    public delegate void AnimalHeard(AbstractAnimal animal);

    private int _radius;
    public AnimalHeard AnimalHeardListeners;
    private SensedActionsDelegate _sensedActionsDelegate;

    private int Radius
    {
      get => _radius;
      set => _radius = Mathf.Clamp(value, 0, int.MaxValue);
    }

    private void Start()
    {
      Radius = 12;

      var actions = new List<ObjectSensedAction>()
      {
        new ObjectSensedAction(obj =>
          {
            if (obj.GetComponent<AbstractAnimal>() is AbstractAnimal animal && NotSelf(animal) && animal.Alive)
              AnimalHeardListeners?.Invoke(animal);

            return false;
          }
        )
      };

      _sensedActionsDelegate = new SensedActionsDelegate(actions);
    }

    private void OnTriggerEnter(Collider other)
    {
      _sensedActionsDelegate.Do(other);
    }

    private bool NotSelf(Component animal)
    {
      return animal.transform.position != transform.parent.position;
    }
  }
}