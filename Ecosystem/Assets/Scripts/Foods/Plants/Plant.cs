using System.Collections.Generic;
using Core;
using Foods.Plants.PlantStates;
using UI;
using UnityEngine;

namespace Foods.Plants
{
  public sealed class Plant : AbstractFood, ITickable, IInspectable
  {
    public delegate void StateChanged(string state);

    private const int DaysAsSeed = 5;
    private const int SaturationPerDay = 10;
    [SerializeField] private Material seedMaterial;
    [SerializeField] private Material growingMaterial;
    [SerializeField] private Material matureMaterial;
    private int _ageInDays;
    private StateMachine<PlantState> _stateMachine;
    public StateChanged StateChangedListeners;
    public bool LeaveSeedState { get; private set; }

    private void Awake()
    {
      MaxSaturation = 100;

      var states = new List<IState<PlantState>>
      {
        new SeedState(this),
        new GrowState(this),
        new MatureState(this)
      };
      _stateMachine = new StateMachine<PlantState>(states, PlantState.Seed);
      _stateMachine.StateChangedListeners += state => StateChangedListeners?.Invoke(state.ToString());
    }

    /// <summary>
    ///   Resets the plant (sets its age to zero and removes its saturation).
    /// </summary>
    public void Reset()
    {
      _ageInDays = 0;
      Saturation = 0;
      LeaveSeedState = false;
    }

    private void Update()
    {
      _stateMachine.Execute();
    }

    public IList<MonoBehaviour> GetStats(bool getStats)
    {
      return PropertyFactory.Create(this);
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
      return _stateMachine.GetCurrentStateEnum() == PlantState.Mature;
    }

    public PlantState GetCurrentStateEnum()
    {
      return _stateMachine.GetCurrentStateEnum();
    }
  }
}