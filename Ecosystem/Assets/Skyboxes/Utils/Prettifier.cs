using System;

namespace Utils
{
  /// <summary>
  ///   Util class for formatting values to pretty strings.
  /// </summary>
  public static class Prettifier
  {
    public static string Round(float value, int decimals)
    {
      var roundedFloat = (float) Math.Round(value * 100f) / 100f;
      return roundedFloat.ToString();
    }
  }
}