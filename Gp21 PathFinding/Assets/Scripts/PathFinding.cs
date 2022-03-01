using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    //Following https://www.youtube.com/watch?v=AKKpPmxx07w
    
    private Grid grid;
    public Transform StartPosition;
    public Transform TargetPosition;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(StartPosition.position, TargetPosition.position);
    }

    void FindPath(Vector3 _StartPos, Vector3 _TargetPos)
    {
        Node startNode = grid.NodeFromWorldPosition(_StartPos);
        Node targetNode = grid.NodeFromWorldPosition(_TargetPos);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedHash = new HashSet<Node>();
        
        OpenList.Add(startNode);

        while (OpenList.Count > 0)
        {
            Node currentNode = OpenList[0];
            for (int i = 0; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < currentNode.FCost || OpenList[i].FCost == currentNode.FCost 
                    && OpenList[i].hCost < currentNode.hCost)
                {
                    currentNode = OpenList[i];
                }
            }

            OpenList.Remove(currentNode);
            ClosedHash.Add(currentNode);

            if (currentNode == targetNode)
            {
                GetFinalPath(startNode, targetNode);
            }

            foreach (Node NeighborNode in grid.GetNeighboringNodes(currentNode))
            {
                if (!NeighborNode.isObstacle || ClosedHash.Contains(NeighborNode))
                    continue;
                int moveCost = currentNode.gCost + GetManhattanDistance(currentNode, NeighborNode);

                if (moveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.gCost = moveCost;
                    NeighborNode.hCost = GetManhattanDistance(NeighborNode, targetNode);
                    NeighborNode.parent = currentNode;

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }
    }

    private void GetFinalPath(Node _startingNode, Node _endNode)
    {
        List<Node> pathComplete = new List<Node>();
        Node currentNode = _endNode;

        while (currentNode != _startingNode)
        {
            pathComplete.Add(currentNode);
            currentNode = currentNode.parent;
        }
        pathComplete.Reverse();
        grid.pathComplete = pathComplete;
    }

    int GetManhattanDistance(Node _nodeA, Node _nodeB)
    {
        int ix = (int) MathF.Abs(_nodeA.gridX - _nodeB.gridX);
        int iy = (int) MathF.Abs(_nodeA.gridY - _nodeB.gridY);
        int iz = (int) MathF.Abs(_nodeA.gridZ - _nodeB.gridZ);

        return ix + iy + iz;
    }
}
