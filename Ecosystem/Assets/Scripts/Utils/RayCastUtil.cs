using UnityEngine;

namespace Utils
{
  public static class RayCastUtil
  {
    private static bool _hasCachedNonRayCastableLayers;
    private static int _nonRayCastableLayers;

    public static int CastableLayers => ~GetDontCast();

    /// <summary>
    ///   Gets the layer mask of the layers that should not be raycasted.
    /// </summary>
    /// <returns>The layer mask of the layers that should not be ray casted.</returns>
    public static int GetDontCast()
    {
      if (_hasCachedNonRayCastableLayers) return _nonRayCastableLayers;

      _hasCachedNonRayCastableLayers = true;
      _nonRayCastableLayers = LayerMask.GetMask(
        "AnimalVision",
        "AnimalHearing",
        "Ignore Raycast",
        "UI",
        "TransparentFX",
        "PostProcessing", 
        "Water"
      );
      return _nonRayCastableLayers;
    }
  }
}