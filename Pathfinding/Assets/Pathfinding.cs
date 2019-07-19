using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    Grid grid;
    private void Awake()
    {
        grid = GetComponent<Grid>();
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
                return;
            }
        }



    }
}
