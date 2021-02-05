using UnityEngine;

namespace Utils
{
  public static class Vector3Util
  {
    public static double getDistance(Vector3 p1, Vector3 p2)
    {
      return (p1 - p2).magnitude;
    }

    public static double getDistance(GameObject g1, GameObject g2)
    {
      return (g1.transform.position - g2.transform.position).magnitude;
    }
    
    public static bool isInRange(GameObject g1, GameObject g2,double distance)
    {
      return (g1.transform.position - g2.transform.position).magnitude <= distance;
    }
    
    public static bool isInRange(Vector3 p1 , Vector3 p2,double distance)
    {
      return (p1 - p2).magnitude <= distance;
    }
  }
}