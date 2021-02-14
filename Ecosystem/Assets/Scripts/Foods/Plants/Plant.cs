using System.Collections.Generic;
using Foods.Plants.PlantStates;
using UnityEngine;

namespace Foods.Plants
{
  public sealed class Plant : AbstractFood, ITickable
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
      _ageInDays++;

      if (_ageInDays >= DaysAsSeed)
        ShouldGrow = true;

      if (ShouldGrow)
        Saturation += GrowthPerDay;
    }

    public void ShowAsSeed()
    {
      SetXMaterial(seedMaterial);
    }

    public void ShowAsGrowing()
    {
      SetXMaterial(growingMaterial);
    }

    public void ShowAsMature()
    {
      SetXMaterial(matureMaterial);
    }

    private void SetXMaterial(Material material)
    {
      var mesh = GetComponent<MeshRenderer>();
      mesh.material = material;
    }

    public override bool CanBeEaten()
    {
      return _currentState.GetStateEnum() == PlantState.Mature;
    }
  }
}