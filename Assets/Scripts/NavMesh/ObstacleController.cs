using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private GameObject squareObstacle;
    [SerializeField] private GameObject cylinderObstacle;

    [SerializeField] private LayerMask groundMask;

    private GameObject currentObstaclePrefab;

    private bool holdingDown = false;
    private Vector3 savedPosition;
    private GameObject heldObstacle;

    private const float OBSTACLE_HEIGHT = 0.2f;


    private void Start()
    {
        currentObstaclePrefab = squareObstacle;
    }
    void Update()
    {
        // On first click
        if(Input.GetMouseButtonDown(0))
        {
            holdingDown = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
            {
                savedPosition = hit.point;
                Debug.Log(savedPosition);
                heldObstacle = Instantiate(currentObstaclePrefab, savedPosition, Quaternion.identity);
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            holdingDown = false;
            savedPosition = Vector3.zero;
            heldObstacle = null;
        }
        if(holdingDown)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
            {
                heldObstacle.transform.position = Vector3.Lerp(hit.point, savedPosition, 0.5f);
                heldObstacle.transform.localScale = new Vector3(Mathf.Abs(hit.point.x - savedPosition.x), OBSTACLE_HEIGHT, Mathf.Abs(hit.point.z - savedPosition.z));
            }
        }
    }
}
