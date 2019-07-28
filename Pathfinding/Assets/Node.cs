using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>{
    public bool walkable;
    public int gCost, hCost;
    public int gridX, gridY, heapIndex, movementPenalty;
    public Vector3 worldPosition;                        
    public Node parent;

    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY, int _penalty)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
        movementPenalty = _penalty;
    }
                        
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node node)
    {
        int compare = fCost.CompareTo(node.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(node.hCost);
        }
        return -compare;
    }

}
