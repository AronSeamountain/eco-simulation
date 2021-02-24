using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GroundGenerator : MonoBehaviour
{
    private Mesh mesh;
    
    private Vector3[] _vertices;
    private int[] _triangles;

    public int xSize = 20;
    public int zSize = 20;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        _vertices = new Vector3[(xSize + 1) * (zSize + 1)];
 
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 1.5f;
                _vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }
        
        _triangles = new int[xSize * zSize * 6];
        int v = 0;
        int t = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                _triangles[t + 0] = v + 0;
                _triangles[t + 1] = v + xSize + 1;
                _triangles[t + 2] = v + 1;
                _triangles[t + 3] = v + 1;
                _triangles[t + 4] = v + xSize + 1;
                _triangles[t + 5] = v + xSize + 2;
                v++;
                t += 6;
            }

            v++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        
        mesh.RecalculateNormals();
    }
    
}
