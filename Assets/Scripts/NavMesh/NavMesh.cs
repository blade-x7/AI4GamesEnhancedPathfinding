using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NavMesh : MonoBehaviour
{
    Vector2[] vertices;
    Vector2[] outputVertices;

    const string OBSTACLE_TAG = "Obstacle";

    public void BakeMesh()
    {
        if(vertices.Length == 0) return;
        List<Vector2> outputList = new();
        foreach (Vector2 v in vertices)
        {
            Collider2D hit = Physics2D.OverlapPoint(v);
            if (hit == null || hit.CompareTag(OBSTACLE_TAG))
            {
                outputList.Add(v);
            }
        }
        outputVertices = outputList.ToArray();
    }
}

[CustomEditor(typeof(NavMesh))]
public class NavMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NavMesh mesh = (NavMesh)target;
        if(GUILayout.Button("Bake Mesh"))
        {
            mesh.BakeMesh();
        }
    }
}