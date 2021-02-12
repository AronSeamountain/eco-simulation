public struct NourishmentSnapshot
{
  public int Saturation { get; set; }
  public int Hydration { get; set; }
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