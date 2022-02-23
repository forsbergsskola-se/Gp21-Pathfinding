using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
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
}
