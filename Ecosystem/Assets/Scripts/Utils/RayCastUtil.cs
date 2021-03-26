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

    public static RaycastHit RayCastLayer(Vector3 startPoint, Vector3 endPoint, int layer)
    {
      var layerMask = 1 << layer; //only looks for one layer
      return rayCastWithMask(startPoint, endPoint, layerMask);
    }

    public static RaycastHit RayCastLayer(Vector3 startPoint, Vector3 endPoint, string[] layers)
    {
      var layerMasks = LayerMask.GetMask(layers);
      return rayCastWithMask(startPoint, endPoint, layerMasks);
      ;
    }

    private static RaycastHit rayCastWithMask(Vector3 startPoint, Vector3 endPoint, int layerMasks)
    {
      var ray = new Ray(startPoint, endPoint);
      Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMasks);
      return hit;
    }
  }
}