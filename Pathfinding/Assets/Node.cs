using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public bool walkable;
    public int gCost, hCost;
    public int gridX, gridY;
    public Vector3 worldPosition;

    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        _gridX = gridX;
        _gridY = gridY;
    }
                        
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
