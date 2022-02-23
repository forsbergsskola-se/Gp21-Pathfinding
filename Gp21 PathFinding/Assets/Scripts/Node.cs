using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    //Following https://www.youtube.com/watch?v=AKKpPmxx07w
    public int gridX; // Xposition in node array
    public int gridY; // Yposition in node array
    public int gridZ; // Zposition in node array
    public int gCost; // Cost of moving one node
    public int hCost; // Distance in cost from current node
    public int FCost{ get { return gCost + hCost; }} // Gcost + hcost ^^

    public bool isObstacle; // Asks if node is obstructed
    public Vector3 position; // Node world position
    public Node parent;

    public Node(bool a_isObstacle, Vector3 a_Pos, int a_gridX, int a_gridY, int a_gridZ)
    {
        isObstacle = a_isObstacle;
        position = a_Pos;
        gridX = a_gridX;
        gridY = a_gridY;
        gridZ = a_gridZ;
    }



}
