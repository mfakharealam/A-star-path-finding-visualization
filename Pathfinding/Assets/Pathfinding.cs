using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour {

    public Transform seeker, target;
    Grid grid;
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            findPath(seeker.position, target.position);
        }
    }

    void findPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

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
                retracePath(startNode, targetNode);
                return;
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

    void retracePath(Node start, Node end)
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
