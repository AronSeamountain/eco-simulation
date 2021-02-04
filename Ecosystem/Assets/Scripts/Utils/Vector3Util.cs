using UnityEngine;

namespace Utils
{
    public static class Vector3Util
    {
        public static double getDistanceBetween2Points(Vector3 p1, Vector3 p2)
        {
            return (p1-p2).magnitude;
        }

        public static double getDistanceBetween2GameObjects(GameObject g1, GameObject g2)
        {
            return (g1.transform.position - g2.transform.position).magnitude;
        }
    }
}