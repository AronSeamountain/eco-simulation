using System.Collections.Generic;
using Foods.Plants.PlantStates;
using UnityEngine;

namespace Foods.Plants
{
  public sealed class Plant : Food, ITickable
  {
    private const int DaysAsSeed = 5;
    private const int GrowthPerDay = 10;
    [SerializeField] private Material seedMaterial;
    [SerializeField] private Material growingMaterial;
    [SerializeField] private Material matureMaterial;
    private int _ageInDays;
    private IGenericState<Plant, PlantState> _currentState;
    private StateMachine<Plant, PlantState> _stateMachine;
    public int MaxSaturation { get; } = 100;

    public bool ShouldGrow { get; private set; }

    private void Start()
    {
      var states = new List<IGenericState<Plant, PlantState>>
      {
        new SeedState(),
        new GrowState(),
        new MatureState()
      };
      _stateMachine = new StateMachine<Plant, PlantState>(states);
      _currentState = _stateMachine.GetCorrelatingState(PlantState.Seed);
      _currentState.Enter(this);
    }

    private void Update()
    {
      var nextState = _currentState.Execute(this);
      if (_currentState.GetStateEnum() != nextState)
      {
        _currentState.Exit(this);
        _currentState = _stateMachine.GetCorrelatingState(nextState);
        _currentState.Enter(this);
      }
    }

    public void Tick()
    {
    }

    public void DayTick()
    {
      // TODO: This is NOT final!!!! Should be in the state machine somehow.
      _ageInDays++;
      if (_ageInDays >= DaysAsSeed) ShouldGrow = true;

      if (ShouldGrow) saturation += GrowthPerDay;
    }

    public void SetSeedMaterial()
    {
      SetXMaterial(seedMaterial);
    }

    public void SetGrowingMaterial()
    {
      SetXMaterial(growingMaterial);
    }

    public void SetMatureMaterial()
    {
      SetXMaterial(matureMaterial);
    }

    private void SetXMaterial(Material material)
    {
      var mesh = GetComponent<MeshRenderer>();
      mesh.material = material;
    }
  }
}