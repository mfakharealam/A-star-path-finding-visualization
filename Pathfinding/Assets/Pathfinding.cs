using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

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
                    int newMovementCostToNeighbour = currNode.gCost + getDistance(currNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = getDistance(neighbour, targetNode);
                        neighbour.parent = currNode;
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
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
        path.Reverse();
        grid.path = path;
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
