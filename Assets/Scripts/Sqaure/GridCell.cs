using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector2 pos;
    public int fCost;
    public int gCost;
    public int hCost;
    public Vector2 connection;
    public bool isWall;

    public GridCell(Vector2 position)
    {
        pos = position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
