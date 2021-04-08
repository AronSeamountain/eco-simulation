using System.Collections.Generic;
using Animal.Sensor.SensorActions;
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
    private SensorActionDelegate _sensorActionDelegate;

    private int Radius
    {
      get => _radius;
      set => _radius = Mathf.Clamp(value, 0, int.MaxValue);
    }

    private void Awake()
    {
      _sensorActionDelegate = new SensorActionDelegate();
    }

    private void Start()
    {
      Radius = 12;

      var actions = new List<SensorAction>()
      {
        HearingActionFactory.CreateAnimalHeardAction(this)
      };
    }

    private void OnTriggerEnter(Collider other)
    {
      _sensorActionDelegate.Do(other);
    }

    public bool NotSelf(Component animal)
    {
      return animal.transform.position != transform.parent.position;
    }

    /// <summary>
    ///   Populates the hearing events for the specific species.
    /// </summary>
    /// <param name="species">The type of species the hearing is attached to.</param>
    public void Config(AnimalSpecies species)
    {
      _sensorActionDelegate.AddAction(HearingActionFactory.CreateAnimalHeardAction(this));
    }
  }
}