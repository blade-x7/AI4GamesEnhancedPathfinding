using UnityEngine;
using UnityEngine.UI;

/*
 * 
 * Created by Arija Hartel (@cocoatehcat)
 * HEAVILY references CatLikeCoding
 * Purpose: Attached to a prefab for each individual hex
 * 
 */

public class HexCell : MonoBehaviour
{
    [SerializeField] private GameObject wallMarker;
    private bool isWall = false;

    [SerializeField]
    HexCell[] neighbors;

    public HexCoords coordinates;
    int distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetWall(bool wall)
    {
        isWall = wall;
        // Create marker designating the current cell as a wall
        Instantiate(wallMarker, transform);
    }

    public HexCell GetNeighbor (HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor (HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public int Distance
    {
        get { return distance; }
        set { distance = value; }
    }

    #region Highlight

    public void DisableHighlight()
    {
        SpriteRenderer highlight = GetComponentInChildren<SpriteRenderer>();
        highlight.enabled = false;
    }

    public void EnableHighlight()
    {
        SpriteRenderer highlight = GetComponentInChildren<SpriteRenderer>();
        highlight.enabled = true;
    }

    public void EnableHighlight(Color color)
    {
        SpriteRenderer highlight = GetComponentInChildren<SpriteRenderer>();
        highlight.color = color;
        highlight.enabled = true;
    }

    #endregion
}
