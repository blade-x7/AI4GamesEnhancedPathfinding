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

    public HexCell cellPrefab;

    HexCell[] cells;

    private void Awake()
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

    private void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = x * 10f;
        position.y = 0f;
        position.z = z * 10f;

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
    }
    public HexCell[] GetCells()
    {
        return cells;
    }
}
