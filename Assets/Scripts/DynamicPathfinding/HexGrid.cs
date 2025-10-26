using System;
using UnityEngine;

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

    [SerializeField] float distApart;

    public HexCell cellPrefab;

    HexCell[] cells;

    private void Awake()
    {

    }
    public void InstantiateGrid()
    {
        cells = new HexCell[width * height];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
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
        // Destroy all children in case they were removed from the cell array at some point
        foreach(Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }
    private void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = x * distApart - ((width - 1) * distApart * 0.5f);
        position.y = 0f;
        position.z = z * distApart - ((height - 1) * distApart * 0.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
    }
    public HexCell[] GetCells()
    {
        return cells;
    }
}
