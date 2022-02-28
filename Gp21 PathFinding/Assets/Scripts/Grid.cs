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
    
    private Node[,,] grid;
    public List<Node> pathComplete;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY, gridSizeZ];
        Vector3 leftBottom = transform.position - Vector3.right * gridWorldSize.x / 2 -
                             Vector3.forward * gridWorldSize.y / 2 - Vector3.up * gridSizeZ / 2;
        for (int y = 0; y < gridSizeX; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 worldPoint = leftBottom + Vector3.right * (x * nodeDiameter + nodeRadius) +
                                         Vector3.forward * (y * nodeDiameter + nodeRadius) + Vector3.up * (z * nodeDiameter + nodeRadius);
                    bool obstacle = !Physics.CheckSphere(worldPoint, nodeRadius, obstacleMask);

                    grid[x, y, z] = new Node(obstacle, worldPoint, x, y, z);
                }
                
            }

            
        }
    }

    public Node NodeFromWorldPosition(Vector3 _WorldPosition)
    {
        float xpoint = ((_WorldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float ypoint = ((_WorldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y);
        float zpoint = ((_WorldPosition.z + gridWorldSize.z / 2) / gridWorldSize.z);

        xpoint = Mathf.Clamp01(xpoint);
        ypoint = Mathf.Clamp01(ypoint);
        zpoint = Mathf.Clamp01(zpoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xpoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * ypoint);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * zpoint);
        
        return grid[x, y, z];
    }

    public List<Node> GetNeighboringNodes(Node _node)
    {
        List<Node> NeighboringNodes = new List<Node>();

        for (int x = Math.Max(_node.gridX -1, 0); x < _node.gridX +2 && x < gridSizeX; x++)
        {
            for (int y = Math.Max(_node.gridY -1, 0); y < _node.gridY +2 && y < gridSizeY; y++)
            {
                for (int z = Math.Max(_node.gridZ -1, 0); z < _node.gridZ +2 && z < gridSizeZ; z++)
                {
                    NeighboringNodes.Add(grid[x, y, z]);
                }
            }
        }
        return NeighboringNodes;
    }
    
    private void OnDrawGizmos()
    {
        //declare gridbox for controlling gridsize
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, gridWorldSize.z));

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
