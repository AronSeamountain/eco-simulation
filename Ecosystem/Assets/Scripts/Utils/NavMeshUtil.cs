using UnityEngine;
using UnityEngine.AI;

namespace Utils
{
  public static class NavMeshUtil
  {
    /// <summary>
    ///   Gets a random point of the nav mesh. Credits to Puck for solution
    ///   (https://answers.unity.com/questions/857827/pick-random-point-on-navmesh.html).
    /// </summary>
    /// <returns>A random point of the nav mesh.</returns>
    public static Vector3 GetRandomLocation()
    {
      var navMeshData = NavMesh.CalculateTriangulation();

      // Pick the first indice of a random triangle in the nav mesh
      var t = Random.Range(0, navMeshData.indices.Length - 3);

      // Select a random point on it
      var point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]],
        navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
      Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

      return point;
    }
  }
}