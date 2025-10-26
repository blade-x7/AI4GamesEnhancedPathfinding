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
        // Destroy the already existing grid
        grid.DestroyGrid();
        // Create a new grid to replace it
        grid.InstantiateGrid();
        // Get the elements of the created grid
        HexCell[] cells = grid.GetCells();
        if(cells.Length == 0) return;
        // Loop through each cell in the grid
        foreach (HexCell h in cells)
        {
            // Beam down a ray from the top to check if it hits an obstacle
            if (Physics.Raycast(h.transform.position + Vector3.up * UP_LENGTH, Vector3.down, Mathf.Infinity, mask))
            {
                // Set the cell to a will if true
                h.SetWall(true);
            }
        }
    }
    public void DestroyMesh()
    {
        // Call grid's destroy
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
        // Button to bake mesh in editor rather than during runtime
        if(GUILayout.Button("Bake Mesh"))
        {
            mesh.BakeMesh();
        }
        // Destroy existing mesh if wanted
        if(GUILayout.Button("Destroy Mesh"))
        {
            mesh.DestroyMesh();
        }
    }
}