using UnityEngine;

/*
 * 
 * Created by Arija Hartel (@cocoatehcat)
 * HEAVILY references CatLikeCoding
 * Purpose: Attached to a prefab for each individual hex
 * 
 */

public class HexCell : MonoBehaviour
{
    private bool isWall = false;
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
        GetComponent<Renderer>().material.color = Color.red;
    }
}
