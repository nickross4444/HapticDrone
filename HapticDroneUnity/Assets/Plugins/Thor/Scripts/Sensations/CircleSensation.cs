using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Circle", menuName = "Sensations/Circle")]
public class CircleSensation : SpeedConstrainableSensation
{
    public float radius;

    public override (Vector3 p, float intensity) EvaluateAt(double seconds)
    {
        Vector3 p = Lerp(GetFraction(seconds));
        return (p, 1);
    }

    public override Vector3 Lerp(double fraction)
    {
        return Shape.CircleAt(fraction, radius);
    }

    protected override float ComputeFrequency()
    {
        return speedTravel / (2 * Mathf.PI * radius);
    }
}
