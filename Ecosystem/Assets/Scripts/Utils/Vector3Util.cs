using UnityEngine;

namespace Utils
{
  public static class Vector3Util
  {
    public static double Distance(Vector3 p1, Vector3 p2)
    {
      return (p1 - p2).magnitude;
    }

    public static double Distance(GameObject g1, GameObject g2)
    {
      return (g1.transform.position - g2.transform.position).magnitude;
    }

    public static bool InRange(GameObject g1, GameObject g2, float distance)
    {
      return (g1.transform.position - g2.transform.position).magnitude <= distance;
    }

    public static bool InRange(Vector3 p1, Vector3 p2, float distance)
    {
      return (p1 - p2).magnitude <= distance;
    }
  }
}