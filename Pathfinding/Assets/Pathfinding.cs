using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour {

    PathRequestManager reqManager;
    Grid grid;
    private void Awake()
    {
        reqManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }
                     
    public void StartPathFind(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(findPath(startPos, endPos));
    }

    IEnumerator findPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false;
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currNode = openSet.RemoveFirst();
                closedSet.Add(currNode);
                if (currNode == targetNode)
                {
                    sw.Stop();
                    print("path found: " + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }
                foreach (Node neighbour in grid.getNeighbours(currNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }
                    int newMovementCostToNeighbour = currNode.gCost + getDistance(currNode, neighbour) + neighbour.movementPenalty;
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = getDistance(neighbour, targetNode);
                        neighbour.parent = currNode;
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }

            }
        }
        yield return null; // wait for 1 frame before returning...
        if (pathSuccess)
        {
            wayPoints = retracePath(startNode, targetNode);
        }
        reqManager.finishedProcessingPath(wayPoints, pathSuccess);
    }

    Vector3[] retracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node currNode = end;
        while (currNode != start)
        {
            path.Add(currNode);
            currNode = currNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)      // waypoints placed only whenever the path changes direction 
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 oldDir = Vector2.zero;
        for (int i = 1; i < path.Count; i++)
        {
            Vector2 newDir = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);     // last two nodes
            if (oldDir != newDir)
            {
                waypoints.Add(path[i - 1].worldPosition);
            }
            oldDir = newDir; 
        }
        return waypoints.ToArray();            
    }

    int getDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if (distX > distY)                                       
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);                     
    }

}
