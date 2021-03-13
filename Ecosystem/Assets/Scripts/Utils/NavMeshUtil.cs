using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Utils
{
  public static class NavMeshUtil
  {
    private const string WalkableLayerName = "Walkable";
    private const int AllLayers = -1;

    public static int WalkableLayer => NavMesh.GetAreaFromName(WalkableLayerName);

    /// <summary>
    ///   Gets a random point of the nav mesh.
    /// </summary>
    /// <returns>A random point of the nav mesh.</returns>
    public static Vector3 GetRandomLocation()
    {
      var navMeshData = NavMesh.CalculateTriangulation();

      var t = Random.Range(0, navMeshData.indices.Length - 3);

      var point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]],
        navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
      point = Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

      return point;
    }

    /// <summary>
    ///   Gets a new point close to the given origin vector. Will have the same y level.
    /// </summary>
    /// <param name="origin">The point to get a point close to.</param>
    /// <param name="radius">The radius to check within.</param>
    /// <exception cref="Exception">If there is no close point on navmesh.</exception>
    /// <returns>A new point close to the given origin vector.</returns>
    public static Vector3 GetRandomClosePoint(Vector3 origin, float radius = 10)
    {
      var offset = new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
      var randomDirection = origin + offset;

      const int lookAtLayers = AllLayers;

      if (NavMesh.SamplePosition(randomDirection, out var hit, radius * 2, lookAtLayers))
        return hit.position;

      throw new Exception("Failed to find a walkable position close to " + randomDirection);
    }

    /// <summary>
    ///   Get point further away from origin vector.
    /// </summary>
    /// <param name="origin">The origin of the animal</param>
    /// <param name="radius">The radius to check within</param>
    /// <returns></returns>
    /// <exception cref="Exception">If there is no point on the navmesh</exception>
    public static Vector3 GetRandomPointFarAway(Vector3 origin, float radius = 100)
    {
      var offset = new Vector3(Random.Range(-radius, radius), 0, Random.Range(50, radius));
      var direction = origin + offset;
      const int lookAtLayers = AllLayers;

      if (NavMesh.SamplePosition(direction, out var hit, radius * 2, lookAtLayers))
        return hit.position;

      throw new Exception("Failed to find point far away " + direction);
    }
  }
}