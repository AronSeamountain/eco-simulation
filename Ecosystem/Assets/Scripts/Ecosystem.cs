using System.Collections.Generic;
using Logger;
using UnityEngine;

public sealed class Ecosystem : MonoBehaviour
{
  /// <summary>
  ///   The amount of time that a "unit" is in.
  /// </summary>
  private const float UnitTimeSeconds = 0.5f;

  private const float UnitsPerDay = 2;
  private int _days;
  private DataLogger _logger;
  private Tick _tickListeners;
  private float _unitsPassed;
  private float _unitTicker;
  private IList<Animal> _animals;

  private void Start()
  {
    _animals = new List<Animal>();
    var animalsInScene = FindObjectsOfType<Animal>(); // Animals should be added via code
    foreach (var animal in animalsInScene)
      _animals.Add(animal);

    _logger = DataLogger.Instance;
    _logger.InitializeLogging();

    foreach (var animal in _animals)
      _tickListeners += animal.Tick;

    _logger.Snapshot(0, _animals);
  }

  private void Update()
  {
    UpdateTick();
  }

  private void UpdateTick()
  {
    _unitTicker += Time.deltaTime;

    if (_unitTicker >= UnitTimeSeconds)
    {
      _unitTicker = 0;
      _unitsPassed++;

      if (_unitsPassed >= UnitsPerDay)
      {
        _unitsPassed = 0;
        _days++;

        _tickListeners?.Invoke();
        _logger.Snapshot(_days, _animals);
      }
    }
  }

  private delegate void Tick();
}