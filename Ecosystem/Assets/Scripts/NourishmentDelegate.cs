/// <summary>
///   A delegate for something that needs to eat and drink.
/// </summary>
public sealed class NourishmentDelegate : ITickable
{
  public delegate void NourishmentChanged(NourishmentSnapshot nourishmentSnapshot);

  public NourishmentChanged NourishmentChangedListeners;
  private int _saturation;
  private int _hydration;

  /// <summary>
  ///   The value for which the animal is considered hungry.
  /// </summary>
  private const int HungrySaturationLevel = 50;

  /// <summary>
  ///   The value for which the animal is considered thirsty.
  /// </summary>
  private const int ThirstyHydrationLevel = 50;

  private const int SaturationDecreasePerUnit = 1;
  private const int HydrationDecreasePerUnit = 1;

  public NourishmentDelegate()
  {
    Saturation = 25;
    Hydration = 25;
    MaxHydration = 50;
    MaxSaturation = 50;
  }

  public int Saturation
  {
    get => _saturation;
    set
    {
      _saturation = value;
      Invoker();
    } 
  }

  public int Hydration
  {
    get => _hydration;
    set
    {
      _hydration = value;
      Invoker();
    } 
  }

  public bool IsHungry => Saturation <= HungrySaturationLevel;
  public bool IsThirsty => Hydration <= ThirstyHydrationLevel;
  public int MaxHydration { get; }
  public int MaxSaturation { get; }
  

  public void Tick()
  {
    Saturation -= SaturationDecreasePerUnit;
    Hydration -= HydrationDecreasePerUnit;
    
    Invoker();
  }

  public void Invoker()
  {
    NourishmentChangedListeners?.Invoke(new NourishmentSnapshot(Saturation, Hydration, MaxSaturation, MaxHydration));
  }
}