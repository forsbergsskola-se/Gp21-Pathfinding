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
    List<Node> NeighboringNodes = new List<Node>();
    public List<Node> GetNeighboringNodes(Node _node)
    {
        //makes a new list every time, bad performance
        NeighboringNodes.Clear();

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

    
    public List<Node> GetNodesInRadius(Vector3 _targetLocation, float _radius)
    {
        List<Node> tempNodes = new List<Node>();
        foreach (Node node in grid)
        {
            if (Vector3.Distance(node.position, _targetLocation) <= _radius)
                tempNodes.Add(node);
        }

        return tempNodes;
    }

    public Node ClosestNodeToLocation(Vector3 _Target, List<Node> nodes = null)
    {
        Node tempNode = null;
        int loopIndex = 0;
        float tempDistance = 0;

        if (nodes == null)
        {
            foreach (Node node in grid)
            {
                if (loopIndex == 0 || (Vector3.Distance(_Target, node.position) < tempDistance))
                {
                    tempDistance = Vector3.Distance(_Target, node.position);
                    tempNode = node;
                }
                loopIndex++;
            }
        }
        else
        {
            foreach (Node node in nodes)
            {
                if (loopIndex == 0 || (Vector3.Distance(_Target, node.position) < tempDistance))
                {
                    tempDistance = Vector3.Distance(_Target, node.position);
                    tempNode = node;
                }
                loopIndex++;
            }
        }
        
        return tempNode;
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
                    //Set Node Color if not obstacle
                    Gizmos.color = Color.gray;
                }
                else
                {
                    //Set Node Color if obstacle
                    Gizmos.color = Color.green;
                }
    
                //if final path is not empty
                if (pathComplete != null)
                {
                    if (pathComplete.Contains(node))
                    {
                        //Set Node Color path
                        Gizmos.color = Color.red;
                    }
                }
                
                //draw node at position of the node
                Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter - distance));
            }
        }
    }
}
