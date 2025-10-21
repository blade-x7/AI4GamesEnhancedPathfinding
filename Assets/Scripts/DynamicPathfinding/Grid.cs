using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{

    // Making the grid procedurally
    public int xSize, ySize;
    private Vector3[] vertices;

    private void Awake()
    {
        StartCoroutine(Generate());
    }

    // Making them populate dynamically to see order for debugging
    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        // placement
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                yield return wait;
            }
        }
    }

    // Using these for testing within the editor
    private void OnDrawGizmos()
    {
        // Making sure it does not implode
        if (vertices == null) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
