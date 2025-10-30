using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/*
 * 
 * Created by Arija Hartel (@cocoatehcat)
 * HEAVILY references CatLikeCoding
 * Purpose: Hex Grid Time!
 * 
 */

public class HexGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;
    public TMP_Text cellLabel;

    public Canvas gridCanvas;

    [SerializeField] float distApart;

    public HexCell cellPrefab;

    public HexMesh hexMesh;

    HexCell[] cells;
    TMP_Text[] labels;

    public Vector3 startPos, endPos;

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    public void InstantiateGrid()
    {
        cells = new HexCell[width * height];
        labels = new TMP_Text[width * height];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }

        hexMesh.LoadMesh();

        hexMesh.Triangulate(cells);

    }
    public void DestroyGrid()
    {
        // Return if the grid doesn't exist yet
        if (cells == null) return;
        // Loop through the cell list and destroy objects there
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] != null)
            {
                DestroyImmediate(cells[i].gameObject);
            }
        }
        // Reset the array
        cells = null;

        // Covering the UI
        if (labels == null) return;

        for (int i = 0; i < labels.Length; i++)
        {
            if (labels[i] != null)
            {
                DestroyImmediate(labels[i].gameObject);
            }
        }

        cells = null;

        /*
         * Trying to fix this as it makes things grumpy
         * 
         * // Destroy all children in case they were removed from the cell array at some point
        foreach(Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
         */
    }
    private void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * distApart);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * distApart);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoords.FromOffsetCoords(x, z);

        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }

        TMP_Text label = labels[i] = Instantiate<TMP_Text>(cellLabel);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSepLines();
    }
    public HexCell[] GetCells()
    {
        return cells;
    }

    public void FindDistancesTo (HexCell cell)
    {
        StopAllCoroutines();
        StartCoroutine(Search(cell));
    }
    
    IEnumerator Search (HexCell cell)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
        }

        WaitForSeconds delay = new WaitForSeconds(1 / 60f);
        Queue<HexCell> frontier = new Queue<HexCell>();
        frontier.Enqueue(cell);

        while (frontier.Count > 0)
        {
            yield return delay;
            HexCell current = frontier.Dequeue();
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = current.GetNeighbor(d);
                if (neighbor != null && neighbor.Distance == int.MaxValue)
                {
                    neighbor.Distance = current.Distance + 1;
                    frontier.Enqueue(neighbor);
                }
            }
        }
    }
}


[CustomEditor(typeof(HexGrid))]
public class HexGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var hexGrid = (HexGrid)target;

        if (GUILayout.Button("Generate Grid", GUILayout.Width(200)))
        {
            hexGrid.DestroyGrid();
            hexGrid.InstantiateGrid();
        }
        if (GUILayout.Button("Calculate Path", GUILayout.Width(200)))
        {
            Debug.Log("Test!");
        }
    }
}