using System;
using System.Collections.Generic;


namespace Foods
{
  public class Plant : Food, ITickable
  {
    private int _ageInDays;
    private GenericState<Plant,PlantState> _currentState;
    private readonly int _daysAsSeed = 5;
    private int _growthPerDay = 10;
    private int maxSaturation = 100; //Saturation when plant is fully grown

    private StateMachineContainer<GenericState<Plant, PlantState>, PlantState, Plant> stateContainer;
    
    public bool ShouldGrow { get; private set; }

    private void Start()
    {
      var seedState = new SeedState();
      _currentState = seedState;
      
      
      var _states = new List<GenericState<Plant, PlantState>>
      {
        seedState,
        new GrowState(),
        new MatureState()
      };
      stateContainer = new StateMachineContainer<GenericState<Plant, PlantState>, PlantState, Plant>(_states);
    }

    private void Update()
    {
      PlantState nextState = _currentState.Execute(this);
      if (_currentState.GetStateEnum() == nextState)
      {
        
      }
      else
      {
        _currentState.Exit(this);
        
      }
     
      
    }

    public int MaxSaturation()
    {
      return maxSaturation;
    }

    public void Tick()
    {
    }
    
    public void DayTick()
    {
     // _currentState.DayTick(this);
      
    }

    public void Grow()
    {
      saturation =+ _growthPerDay;
    }

    public void IncreaseAge()
    {
      _ageInDays++;
      if (_ageInDays >= _daysAsSeed) ShouldGrow = true;
    }
  }
}