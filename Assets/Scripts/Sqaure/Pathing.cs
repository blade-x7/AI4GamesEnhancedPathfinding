using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Pathing : MonoBehaviour
{
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private float cellHeight = 100.0f;
    [SerializeField] private float cellWidth = 100.0f;

    [SerializeField] private bool generatePath;
    [SerializeField] private bool generateGrid;
    [SerializeField] private bool visualGrid;

    [SerializeField] private Vector2 startingPosition = new Vector2(0,0);
    [SerializeField] private Vector2 endingPosition = new Vector2(5, 7);

    private bool pathGenerated;
    private bool gridGenerated;

    private Dictionary<Vector2, CellTest> cells;

    [SerializeField] private List<Vector2> cellsToSearch;
    [SerializeField] private List<Vector2> searchedCells;
    [SerializeField] private List<Vector2> finalPath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (generateGrid) 
        {
            GenerateGrid();
            gridGenerated = true;
            generateGrid = false;
            finalPath.Clear();
        }
        else if (generatePath && gridGenerated)
        {
            FindPath(startingPosition, endingPosition);
            generatePath = false;
        }
    }

    // Makes grid
    private void GenerateGrid()
    {
        cells = new Dictionary<Vector2, CellTest>();

        for (float x = 0; x < gridWidth; x += cellWidth)
        {
            for (float y = 0; y < gridHeight; y += cellHeight)
            {
                Vector2 pos = new Vector2(x, y);
                cells.Add(pos, new CellTest(pos));
            }
        }

        // Randomly generates walls
        for (int i = 0; i < 40; i++)
        {
            Vector2 pos = new Vector2(Random.Range(0, gridWidth), Random.Range(0, gridHeight));
            cells[pos].isWall = true;
        }
    }

    // Calculate path
    private void FindPath(Vector2 startPos, Vector2 endPos)
    {
        searchedCells = new List<Vector2>();
        cellsToSearch = new List<Vector2> { startPos };
        finalPath = new List<Vector2>();

        CellTest startCell = cells[startPos];
        startCell.gCost = 0;
        startCell.hCost = GetDistance(startPos, endPos);
        startCell.fCost = GetDistance(startPos, endPos);

        while (cellsToSearch.Count > 0)
        {
            Vector2 cellToSearch = cellsToSearch[0];

            foreach (Vector2 pos in cellsToSearch)
            {
                CellTest c = cells[pos];
                if (c.fCost < cells[cellToSearch].fCost || c.fCost == cells[cellToSearch].fCost && c.hCost == cells[cellToSearch].hCost) 
                {
                    cellToSearch = pos;
                }
            }

            cellsToSearch.Remove(cellToSearch);
            searchedCells.Add(cellToSearch);

            if (cellToSearch == endPos)
            {
                // Path is found!
                CellTest pathCell = cells[endPos];

                while (pathCell.pos != startPos)
                {
                    finalPath.Add(pathCell.pos);
                    pathCell = cells[pathCell.connection];
                }

                finalPath.Add(startPos);
                return;
            }

            SearchCellNeighbors(cellToSearch, endPos);
        }
    }

    private void SearchCellNeighbors(Vector2 cellPos, Vector2 endPos)
    {
        for (float x = cellPos.x - cellWidth; x <= cellWidth + cellPos.x; x += cellWidth)
        {
            for (float y = cellPos.y - cellHeight; y <= cellHeight + cellPos.y; y += cellHeight)
            {
                Vector2 neighborPos = new Vector2(x, y);
                // Checks
                if (cells.TryGetValue(neighborPos, out CellTest c) && !searchedCells.Contains(neighborPos) && !cells[neighborPos].isWall)
                {
                    int GCostToNeighbor = cells[cellPos].gCost + GetDistance(cellPos, neighborPos);

                    if (GCostToNeighbor < cells[neighborPos].gCost)
                    {
                        CellTest neighborNode = cells[neighborPos];

                        neighborNode.connection = cellPos;
                        neighborNode.gCost = GCostToNeighbor;
                        neighborNode.hCost = GetDistance(neighborPos, endPos);
                        neighborNode.fCost = neighborNode.gCost + neighborNode.hCost;

                        if (!cellsToSearch.Contains(neighborPos))
                        {
                            cellsToSearch.Add(neighborPos);
                        }
                    }
                }
            }
        }
    }

    private int GetDistance(Vector2 pos1, Vector2 pos2)
    {
        Vector2Int dist = new Vector2Int(Mathf.Abs((int)pos1.x - (int)pos2.x), Mathf.Abs((int)pos1.y - (int)pos2.y));
        // How many horixontal/vertical
        int lowest = Mathf.Min(dist.x, dist.y);
        int highest = Mathf.Max(dist.x, dist.y);

        int horizontalMoves = highest - lowest;

        // diagonal distance is 1.4, multiplying by 10 to make less annoying
        return lowest * 14 + horizontalMoves * 10;
    }

    private void OnDrawGizmos()
    {
        if (!visualGrid || cells == null)
        {
            return;
        }

        foreach (KeyValuePair<Vector2, CellTest> kvp in cells)
        {
            if (!kvp.Value.isWall)
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.magenta;
            }

            if (finalPath.Contains(kvp.Key))
            {
                Gizmos.color = Color.black;
            }

            Gizmos.DrawCube(kvp.Key + (Vector2)transform.position, new Vector3(cellWidth, cellHeight));
        }
    }

    // testing abstract
    private class CellTest
    {
        public Vector2 pos;
        public int fCost = int.MaxValue;
        public int gCost = int.MaxValue;
        public int hCost = int.MaxValue;
        public Vector2 connection;
        public bool isWall;

        public CellTest(Vector2 position)
        {
            pos = position;
        }
    };
}


