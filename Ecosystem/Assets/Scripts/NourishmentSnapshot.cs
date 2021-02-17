public struct NourishmentSnapshot
{
  public float Saturation { get; }
  public float Hydration { get; }
  public float MaxSaturation { get; }
  public float MaxHydration { get; }

  public NourishmentSnapshot(float saturation, float hydration, float maxSaturation, float maxHydration)
  {
    Saturation = saturation;
    Hydration = hydration;
    MaxSaturation = maxSaturation;
    MaxHydration = maxHydration;
  }
}