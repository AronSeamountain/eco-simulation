using System;
using System.Collections.Generic;
using Animal;
using Core;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Utils
{
  public static class NavMeshUtil
  {
    private const string WalkableLayerName = "Walkable";
    private const int AllLayers = -1;
    public static List<MonoBehaviour>[,] WalkablePointMatrix;
    public static IList<List<MonoBehaviour>> WalkablePointAdjacencyList;

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
    /// <param name="distance">The distance to run.</param>
    /// <exception cref="Exception">If there is no close point on navmesh.</exception>
    /// <returns>A new point close to the given origin vector.</returns>
    public static Vector3 GetRandomClosePoint(Vector3 origin, float radius = 10, float distance = 5)
    {
      var offset = new Vector3(Random.Range(-radius, radius), 0, Random.Range(-distance, distance));
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
    public static Vector3 GetRandomPointFarAway(Vector3 origin, float radius = 40)
    {
      var offset =
        new Vector3(Random.Range((float) (-radius * (4 * Math.PI) / 3), (float) (radius * (5 * Math.PI) / 3)), 0,
          Random.Range(-10, 10));
      var direction = origin + offset;
      LayerMask mask = LayerMask.GetMask("Default");

      if (NavMesh.SamplePosition(direction, out var hit, radius * 6, mask))
        return hit.position;

      throw new Exception("Failed to find point far away " + direction);
    }


    public static MonoBehaviour getRandomWalkablePoint()
    {
      int index = Random.Range(0, WorldMatrix.WalkablePoints.Count);
      return WorldMatrix.WalkablePoints[index];
    }

    public static MonoBehaviour getRandomWalkablePointInMatrix(Vector3 selfPos)
    {
      var x = (int) selfPos.x / WorldMatrix.WalkableMatrixBoxSize;
      var z = (int) selfPos.z / WorldMatrix.WalkableMatrixBoxSize;

      var listWithEntry = findMatrixEntryWithWalkablePoint(x, z);

      var index = Random.Range(0, listWithEntry.Count);

      return WalkablePointMatrix[x, z][index];
    }

    private static List<MonoBehaviour> findMatrixEntryWithWalkablePoint(int x, int z)
    {
      return WalkablePointMatrix[x, z];
    }

    public static MonoBehaviour getRandomWalkablePointAdjacencyList(Vector3 selfPos)
    {
      var x = (int) selfPos.x / WorldMatrix.WalkableMatrixBoxSize;
      var z = (int) selfPos.z / WorldMatrix.WalkableMatrixBoxSize;


      var adjacentWalkablePoints = WalkablePointAdjacencyList[x * WorldMatrix.amountOfBoxesPerMatrixLayer + z];
      var walkablePoint = adjacentWalkablePoints[Random.Range(0, adjacentWalkablePoints.Count)];

      return walkablePoint;
    }
  }
}