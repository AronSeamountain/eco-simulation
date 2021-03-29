using UnityEngine;

namespace Core
{
  public sealed class FpsDelegate
  {
    public int FramesSinceLastSnapshot { get; private set; }
    public float TimeSinceLastSnapshot { get; private set; }
    public float AvgSinceLastSnapshot => FramesSinceLastSnapshot / TimeSinceLastSnapshot;

    public void FramePassed()
    {
      FramesSinceLastSnapshot++;
      TimeSinceLastSnapshot += Time.deltaTime;
    }

    public void Reset()
    {
      TimeSinceLastSnapshot = 0;
      FramesSinceLastSnapshot = 0;
    }
  }
}