using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface ISensation
{
    /// <summary>
    /// Compute the control point position and intensity at a moment in time
    /// </summary>
    /// <param name="seconds">The number of seconds since the start of the game</param>
    /// <returns>A tuple containing the control point position as a vector and the intensity as a float</returns>
    (Vector3 p, float intensity) EvaluateAt(double seconds);

    /// <summary>
    /// Compute the point position in the shape. Use this function to draw the sensation or to generate the points
    /// for the sensation.
    /// </summary>
    /// <param name="fraction">A value between 0 and 1 representing the relative position in the shape</param>
    /// <returns>The position of the point in the shape as a vector</returns>
    Vector3 Lerp(double fraction);

    List<Vector3> GetLinePoints(int number);
}
