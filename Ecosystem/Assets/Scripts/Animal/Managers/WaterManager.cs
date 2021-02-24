﻿using Foods;
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
      ClosestKnownWater = water;

      WaterUpdateListeners?.Invoke(ClosestKnownWater);
    }
  }
}