using System.Collections.Generic;
using Core;
using Foods.Plants.PlantStates;
using UI;
using UnityEngine;

namespace Foods.Plants
{
  public sealed class Plant : AbstractFood, ITickable, IStatable
  {
    private const int DaysAsSeed = 5;
    private const int SaturationPerDay = 10;
    [SerializeField] private Material seedMaterial;
    [SerializeField] private Material growingMaterial;
    [SerializeField] private Material matureMaterial;
    private int _ageInDays;
    private IState<Plant, PlantState> _currentState;
    private StateMachine<Plant, PlantState> _stateMachine;
    public bool LeaveSeedState { get; private set; }

    /// <summary>
    ///   Resets the plant (sets its age to zero and removes its saturation).
    /// </summary>
    public void Reset()
    {
      _ageInDays = 0;
      Saturation = 0;
      LeaveSeedState = false;
    }

    private void Awake()
    {
      MaxSaturation = 100;

      var states = new List<IState<Plant, PlantState>>
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

    public IList<GameObject> GetStats(bool getStats)
    {
      return PropertyFactory.MakePlantObjects(this);
    }

    public void Tick()
    {
    }

    public void DayTick()
    {
      _ageInDays++;

      if (_ageInDays >= DaysAsSeed)
        LeaveSeedState = true;

      if (LeaveSeedState)
        Saturation += SaturationPerDay;
    }

    public void ShowAsSeed()
    {
      SetMaterial(seedMaterial);
    }

    public void ShowAsGrowing()
    {
      SetMaterial(growingMaterial);
    }

    public void ShowAsMature()
    {
      SetMaterial(matureMaterial);
    }

    private void SetMaterial(Material material)
    {
      var mesh = GetComponent<MeshRenderer>();
      mesh.material = material;
    }

    protected override void FoodFullyConsumed()
    {
    }

    public override bool CanBeEaten()
    {
      return _currentState.GetStateEnum() == PlantState.Mature;
    }
  }
}