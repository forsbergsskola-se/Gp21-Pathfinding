using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //Following https://www.youtube.com/watch?v=AKKpPmxx07w

    public Transform startPosition;
    public LayerMask obstacleMask;
    public Vector3 gridWorldSize;
    public float nodeRadius;
    public float distance;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY, gridSizeZ;
    
    private Node[,] grid;
    public List<Node> pathComplete;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
       // gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 leftBottom = transform.position - Vector3.right * gridWorldSize.x / 2 -
                             Vector3.forward * gridWorldSize.y / 2;
        for (int y = 0; y < gridSizeX; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = leftBottom + Vector3.right * (x * nodeDiameter + nodeRadius) +
                                     Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool obstacle = !Physics.CheckSphere(worldPoint, nodeRadius, obstacleMask);

                grid[y, x] = new Node(obstacle, worldPoint, x, y);
            }
        }
    }

    private void OnDrawGizmos()
    {
        //declare gridbox for controlling gridsize
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        //Checking if grid has been initialized, to avoid null reference errors
        if (grid != null)
        {
            foreach (Node node in grid)
            {
                //Checking if current node is an obstacle and if so, set color accordingly
                if (node.isObstacle)
                {
                    Gizmos.color = Color.gray;
                }
                else
                {
                    Gizmos.color = Color.green;
                }
    
                //if final path is not empty
                if (pathComplete != null)
                {
                    Gizmos.color = Color.red;
                }
                
                //draw node at position of the node
                Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter - distance));
            }
        }
    }
}
