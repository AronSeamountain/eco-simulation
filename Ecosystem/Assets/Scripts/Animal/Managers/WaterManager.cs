using Foods;
using UnityEngine;

namespace Animal.Managers
{
  public class WaterManager : MonoBehaviour
  {
    public delegate void KnownClosestWaterChanged(Water water);

    [SerializeField] private VisualDetector visualDetector;

    public KnownClosestWaterChanged WaterUpdateListeners;
    public Water ClosestKnownWater { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
      visualDetector.WaterFoundListeners += OnWaterFound;
    }

    private void OnWaterFound(Water water)
    {
      if (water == null) return;
      if (!ClosestKnownWater)
        ClosestKnownWater = water;
      else if (DistanceTo(water) < DistanceTo(ClosestKnownWater))
        ClosestKnownWater = water;
      else
        return;

      WaterUpdateListeners?.Invoke(ClosestKnownWater);
    }

    private float DistanceTo(Water water)
    {
      return Vector3.Distance(water.transform.position, transform.position);
    }
  }
}