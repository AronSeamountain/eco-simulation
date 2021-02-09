using System.Collections.Generic;

namespace Foods
{
  public class Plant : Food, ITickable
  {
    private int _ageInDays;
    private IPlantState _currentState;
    private readonly int _daysAsSeed = 5;
    private IList<IPlantState> _states;
    public bool ShouldGrow { get; private set; }

    private void Start()
    {
      var seedState = new SeedState();
      _currentState = seedState;
      _states = new List<IPlantState>
      {
        seedState,
        new GrowState(),
        new MatureState()
      };
    }

    public void Tick()
    {
    }

    public void DayTick()
    {
      _ageInDays++;
      if (_ageInDays >= _daysAsSeed) ShouldGrow = true;
    }
  }
}