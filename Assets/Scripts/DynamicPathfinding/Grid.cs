using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    // Making the grid procedurally
    public int xSize, ySize;
    private Vector3[] vertices;
    private Mesh mesh;

    private void Awake()
    {
        StartCoroutine(Generate());
    }

    // Making them populate dynamically to see order for debugging
    public IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        
        // Reset for regen
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        // placement points
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                yield return wait;
            }
        }

        // Generating actual mesh
        mesh.vertices = vertices;
        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
                mesh.triangles = triangles;
                yield return wait;
            }
            
        }
        
    }

    // Using these for testing within the editor
    private void OnDrawGizmos()
    {
        // Making sure it does not implode
        if (vertices == null) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}

// Making a custom Inspector bit to reload mesh
[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var grid = (Grid)target;

        EditorGUILayout.LabelField("Grid Settings");

        grid.xSize = EditorGUILayout.IntField("X Size:", grid.xSize);
        grid.ySize = EditorGUILayout.IntField("Y Size:", grid.ySize);

        if (GUILayout.Button("Generate Mesh", GUILayout.Width(200)))
        {
            grid.StartCoroutine(grid.Generate());
        }

    }
}