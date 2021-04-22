using Animal.Sensor;
using Foods;
using UnityEngine;

namespace Animal.Managers
{
  public class WaterManager : MonoBehaviour
  {
    public delegate void KnownClosestWaterChanged(Water water);
    
    private AbstractAnimal _animal;
    [SerializeField] private Vision vision;

    public KnownClosestWaterChanged WaterUpdateListeners;
    public Water ClosestKnownWater { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
      vision.WaterFoundListeners += OnWaterFound;
      
    }

    public void InitWaterManager(AbstractAnimal animal)
    {
      _animal = animal;
    }
    private void OnWaterFound(Water water)
    {
      if (water == null) return;
      if (!ClosestKnownWater)
        ClosestKnownWater = water;
      else if (DistanceTo(water) < DistanceTo(ClosestKnownWater) || _animal.HasForgottenWater)
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