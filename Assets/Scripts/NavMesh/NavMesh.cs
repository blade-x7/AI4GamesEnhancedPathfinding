using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(HexGrid))]
public class NavMesh : MonoBehaviour
{
    [SerializeField] private HexGrid grid;
    [SerializeField] private LayerMask mask;

    const float UP_LENGTH = 5f;

    public void BakeMesh()
    {
        grid.DestroyGrid();
        grid.InstantiateGrid();
        HexCell[] cells = grid.GetCells();
        if(cells.Length == 0) return;
        foreach (HexCell h in cells)
        {
            if (Physics.Raycast(h.transform.position + Vector3.up * UP_LENGTH, Vector3.down, Mathf.Infinity, mask))
            {
                h.SetWall(true);
            }
        }
    }
    public void DestroyMesh()
    {
        grid.DestroyGrid();
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
        if(GUILayout.Button("Destroy Mesh"))
        {
            mesh.DestroyMesh();
        }
    }
}