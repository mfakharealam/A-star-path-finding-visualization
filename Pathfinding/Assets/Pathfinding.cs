using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    public Transform seeker, target;
    Grid grid;
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        findPath(seeker.position, target.position);
    }

    void findPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currNode = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currNode.fCost || openSet[i].fCost == currNode.fCost && openSet[i].hCost < currNode.hCost)
                {
                    currNode = openSet[i];
                }

            }
            openSet.Remove(currNode);
            closedSet.Add(currNode);
            if (currNode == targetNode)
            {
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
