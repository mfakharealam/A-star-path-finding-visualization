using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path {

    public readonly Vector3[] lookPoints;
    public readonly Line[] turnBoundaries;
    public readonly int finishLineIndex;

    public Path(Vector3[] waypoints, Vector3 startPos, float turnDist)
    {
        lookPoints = waypoints;
        turnBoundaries = new Line[lookPoints.Length];
        finishLineIndex = turnBoundaries.Length - 1;

        Vector2 prevPoint = Vec3ToVec2(startPos);

        for (int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currPoint = Vec3ToVec2(lookPoints[i]);
            Vector2 directionToCurrPoint = (currPoint - prevPoint).normalized;
            Vector2 turnBoundaryPoint = (i == finishLineIndex) ? currPoint : currPoint - directionToCurrPoint * turnDist;
            turnBoundaries[i] = new Line(turnBoundaryPoint, prevPoint - directionToCurrPoint * turnDist);
            prevPoint = turnBoundaryPoint;
            
        }

    }

    Vector2 Vec3ToVec2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }
}
