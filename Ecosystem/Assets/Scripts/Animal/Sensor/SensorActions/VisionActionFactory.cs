using Foods;

namespace Animal.Sensor.SensorActions
{
  public static class VisionActionFactory
  {
    public static SensorAction CreateWaterSeenAction(Vision vision)
    {
      return new SensorAction(obj =>
      {
        if (obj.GetComponent<Water>() is Water water)
        {
          vision.WaterFoundListeners?.Invoke(water);
          return true;
        }

        return false;
      });
    }
  }
}