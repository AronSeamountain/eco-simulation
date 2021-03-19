using System.Collections.Generic;
using Core;
using Foods.Plants.PlantStates;
using UI;
using UI.Properties;
using UnityEngine;

namespace Foods.Plants
{
  public sealed class Plant : AbstractFood, ITickable, IInspectable
  {
    public delegate void StateChanged(string state);

    // plant can be alive for 5 in-game days, then i has to start over as seed again
    private const int MaxHoursAlive = 120;
    private const int HoursAsSeed = 24;
    private const int SaturationPerHour = 3;
    [SerializeField] private Material seedMaterial;
    [SerializeField] private Material growingMaterial;
    [SerializeField] private Material matureMaterial;
    private Renderer _meshRend;
    private StateMachine<PlantState> _stateMachine;
    public StateChanged StateChangedListeners;
    public int AgeInHours { get; set; }
    public bool LeaveSeedState { get; private set; }

    private void Awake()
    {
      _meshRend = GetComponent<MeshRenderer>();
      MaxSaturation = 100;

      var states = new List<IState<PlantState>>
      {
        new SeedState(this),
        new GrowState(this),
        new MatureState(this)
      };
      _stateMachine = new StateMachine<PlantState>(states, PlantState.Mature);
      _stateMachine.StateChangedListeners += state => StateChangedListeners?.Invoke(state.ToString());
    }

    /// <summary>
    ///   Resets the plant (sets its age to zero and removes its saturation).
    /// </summary>
    public void Reset()
    {
      AgeInHours = 0;
      Saturation = 0;
      LeaveSeedState = false;
    }

    private void Update()
    {
      _stateMachine.Execute();
    }

    public IEnumerable<AbstractProperty> GetProperties()
    {
      return PropertiesFactory.Create(this);
    }

    public void ShowGizmos(bool show)
    {
    }

    public void HourTick()
    {
      AgeInHours++;

      if (AgeInHours >= HoursAsSeed)
        LeaveSeedState = true;

      if (LeaveSeedState)
        Saturation += SaturationPerHour;

      if (AgeInHours >= MaxHoursAlive) Reset();
    }

    public void DayTick()
    {
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
      var mat = _meshRend.materials;
      mat[1] = material;
      _meshRend.materials = mat;
    }

    protected override void FoodFullyConsumed()
    {
    }

    public override bool CanBeEatenSoon()
    {
      return _stateMachine.GetCurrentStateEnum() == PlantState.Grow;
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