using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Core
{
  public static class WorldMatrix
  {
    public const int WalkablePointsAmountPerBox = 3;
    public static int amountOfBoxesPerMatrixLayer;
    public const int WalkableMatrixBoxSize = 25;
    public static int WorldSize;
    public static IList<MonoBehaviour> WalkablePoints;


    public static List<MonoBehaviour>[,] InitMatrix()
    {
      WalkablePoints = new List<MonoBehaviour>();
      amountOfBoxesPerMatrixLayer = (int) Math.Ceiling(WorldSize / (float) WalkableMatrixBoxSize);
      List<MonoBehaviour>[,] matrix = new List<MonoBehaviour>[amountOfBoxesPerMatrixLayer, amountOfBoxesPerMatrixLayer];
      for (int i = 0; i < matrix.GetLength(0); i++)
      {
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
          matrix[i, j] = new List<MonoBehaviour>();
        }
      }

      return matrix;
    }

    public static void AddWalkablePointsToMatrix(List<MonoBehaviour>[,] matrix)
    {
      foreach (var wp in WalkablePoints)
      {
        int x = (int) Mathf.Floor(wp.gameObject.transform.position.x / WalkableMatrixBoxSize);
        int z = (int) Mathf.Floor(wp.gameObject.transform.position.z / WalkableMatrixBoxSize);

        matrix[x, z].Add(wp);
      }

      NavMeshUtil.WalkablePointMatrix = matrix;
    }

    public static void PopulateAdjacencyList(List<MonoBehaviour>[,] matrix)
    {
      IList<List<MonoBehaviour>> adjacencyList =
        new List<List<MonoBehaviour>>();


      for (int i = 0; i < matrix.GetLength(0); i++)
      {
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
          var tempList = new List<MonoBehaviour>();
          if (j > 0)
            foreach (var monoBehaviour in matrix[i, j - 1]) // adds all from matrix entry to the left
              tempList.Add(monoBehaviour);
          if (j < matrix.GetLength(1) - 1)
          {
            foreach (var monoBehaviour in matrix[i, j + 1])
            {
              // adds all from matrix entry to the right
              tempList.Add(monoBehaviour);
            }
          }

          if (i > 0)
            foreach (var monoBehaviour in matrix[i - 1, j]) // adds all from matrix entry above
              tempList.Add(monoBehaviour);

          if (i < matrix.GetLength(0) - 1)
            foreach (var monoBehaviour in matrix[i + 1, j]) // adds all from matrix entry below
              tempList.Add(monoBehaviour);
          //CORNERS-----------------------------
          if (i > 0 && j > 0)
            foreach (var monoBehaviour in matrix[i - 1, j - 1]) // adds all from matrix top left
              tempList.Add(monoBehaviour);


          if (i > 0 && j < matrix.GetLength(1) - 1)
            foreach (var monoBehaviour in matrix[i - 1, j + 1]) // adds all from matrix top right
              tempList.Add(monoBehaviour);

          if (i < matrix.GetLength(0) - 1 && j > 0)
            foreach (var monoBehaviour in matrix[i + 1, j - 1]) // adds all from matrix bottom left
              tempList.Add(monoBehaviour);

          if (i < matrix.GetLength(0) - 1 && j < matrix.GetLength(1) - 1)
            foreach (var monoBehaviour in matrix[i + 1, j + 1]) // adds all from matrix bottom right
              tempList.Add(monoBehaviour);

          if (tempList.Count < 1)
            Debug.LogError(
              "Error with Worldmatrix instantiation. All entries in the matrix does not have a walkable point");
          adjacencyList.Add(tempList);
        }
      }

      NavMeshUtil.WalkablePointAdjacencyList = adjacencyList;
    }
  }
}