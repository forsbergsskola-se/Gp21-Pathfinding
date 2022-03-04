using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    //Following https://www.youtube.com/watch?v=AKKpPmxx07w
    List<Node> OpenList = new List<Node>();
    HashSet<Node> ClosedHash = new HashSet<Node>();
    private Grid grid;
    public Transform StartPosition;
    public Transform TargetPosition;
    private Vector3 LastTargetNodePosition;
    private Vector3 lastTargetPosition;
    public List<Node> closestNodesToTarget;
    private void Start()
    {
        grid = GetComponent<Grid>();
        StartCoroutine(Timer());

    }

    IEnumerator Timer()
    {
        FindPath(StartPosition.position, TargetPosition.position);
        yield return new WaitForSeconds(1);
        StartCoroutine(Timer());
    } 
    

    void FindPath(Vector3 _StartPos, Vector3 _TargetPos)
    {
        if (Vector3.Distance(lastTargetPosition, _TargetPos) > 2)
        {
            lastTargetPosition = _TargetPos;
            closestNodesToTarget = grid.GetNodesInRadius(_TargetPos, 2);
        }
        
        if (LastTargetNodePosition == grid.ClosestNodeToLocation(_TargetPos, closestNodesToTarget).position)
            return;

        LastTargetNodePosition = grid.ClosestNodeToLocation(_TargetPos).position;

        Node startNode = grid.NodeFromWorldPosition(_StartPos);
        Node targetNode = grid.NodeFromWorldPosition(_TargetPos);
        
        OpenList.Clear();
        ClosedHash.Clear();
        //switch to priority queue.
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
