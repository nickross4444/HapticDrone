using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Line", menuName = "Sensations/Line")]
public class LineSensation : SpeedConstrainableSensation
{
    public Vector3 start;
    public Vector3 stop;

    public bool playBackAndForth = true;

    public override bool IsClosedShape { get { return false; } }

    public override (Vector3 p, float intensity) EvaluateAt(double seconds)
    {
        return (Lerp(GetFraction(seconds)), 1);
    }

    public override Vector3 Lerp(double fraction)
    {
        if (playBackAndForth)
        {
            fraction = fraction <= 0.5 ? fraction * 2 : 1 - ((fraction - 0.5) * 2);
        }
        return Shape.LineAt(fraction, start, stop);
    }

    protected override float ComputeFrequency()
    {
        return speedTravel / (Vector3.Distance(start, stop) * (playBackAndForth ? 2 : 1));
    }
}
