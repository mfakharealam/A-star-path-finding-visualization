using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line {

    const float verticalLineGradient = 1e5f;
    bool approachSide;
    float gradient, y_intercept, gradientPerpendicular;
    Vector2 pointOnLine_1, pointOnLine_2;

    public Line(Vector2 pointOnLine, Vector2 pointPerpToLine)
    {
        float dx = pointOnLine.x - pointPerpToLine.x;
        float dy = pointOnLine.y - pointPerpToLine.y;

        if (dx == 0)
        {
            gradientPerpendicular = verticalLineGradient;
        }
        else
            gradientPerpendicular = dy / dx;

        if (gradientPerpendicular == 0)
        {
            gradient = verticalLineGradient;
        }
        else
            gradient = -1 / gradientPerpendicular;

        y_intercept = pointOnLine.y - gradient * pointOnLine.x; // c = y - mx

        pointOnLine_1 = pointOnLine;
        pointOnLine_2 = pointOnLine + new Vector2(1, gradient);

        approachSide = false;
        approachSide = GetSide(pointPerpToLine);
        
    }

    bool GetSide(Vector2 p)
    {
        return (p.x - pointOnLine_1.x) * (pointOnLine_2.y - pointOnLine_1.y) > (p.y - pointOnLine_1.y) * (pointOnLine_2.x - pointOnLine_1.x);
    }

    public bool hasCrossedLine(Vector2 p)
    {
        return GetSide(p) != approachSide;
    }
}
