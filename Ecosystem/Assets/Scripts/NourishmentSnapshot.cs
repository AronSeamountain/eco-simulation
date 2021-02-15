public struct NourishmentSnapshot
{
  public int Saturation { get; }
  public int Hydration { get; }
  public int MaxSaturation { get; }
  public int MaxHydration { get; }

  public NourishmentSnapshot(int saturation, int hydration, int maxSaturation, int maxHydration)
  {
    Saturation = saturation;
    Hydration = hydration;
    MaxSaturation = maxSaturation;
    MaxHydration = maxHydration;
  }
}