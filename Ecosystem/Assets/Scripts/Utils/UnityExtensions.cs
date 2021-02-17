using UnityEngine;

namespace Utils
{
  public static class UnityExtensions
  {
    /// <summary>
    ///   Checks if the provided layer is in the layer mask. Credits to Mikael-H (https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html).
    /// </summary>
    /// <param name="mask">The mask to check if the layer is in.</param>
    /// <param name="layer">The layer to check.</param>
    /// <returns>True if the provided layer is in the layer mask.</returns>
    public static bool Contains(this LayerMask mask, int layer)
    {
      return mask == (mask | (1 << layer));
    }
  }
}