using System.Collections.Generic;
using UnityEngine;

namespace Foods
{
  public class Plant : Food, ITickable
  {
    private const int DaysAsSeed = 5;
    private const int GrowthPerDay = 10;
    private int _ageInDays;
    private IGenericState<Plant, PlantState> _currentState;
    private StateMachineContainer<Plant, PlantState> _stateContainer;
    public int MaxSaturation { get; } = 100;

    public bool ShouldGrow { get; private set; }

    private void Start()
    {
      var seedState = new SeedState();
      _currentState = seedState;

      var states = new List<IGenericState<Plant, PlantState>>
      {
        seedState,
        new GrowState(),
        new MatureState()
      };
      _stateContainer = new StateMachineContainer<Plant, PlantState>(states);
    }

    private void Update()
    {
      var nextState = _currentState.Execute(this);
      if (_currentState.GetStateEnum() != nextState)
      {
        _currentState.Exit(this);
        _currentState = _stateContainer.GetCorrelatingState(nextState);
        _currentState.Enter(this);
      }
    }

    public void Tick()
    {
    }

    public void DayTick()
    {
      //_currentState.DayTick(this);
      Debug.Log("DAY-tick works");
    }

    public void Grow()
    {
      saturation += GrowthPerDay;
      Debug.Log("PLANTS ARE GROWING - saturation " + saturation);
    }

    public void IncreaseAge()
    {
      _ageInDays++;
      if (_ageInDays >= DaysAsSeed) ShouldGrow = true;
    }
  }
}