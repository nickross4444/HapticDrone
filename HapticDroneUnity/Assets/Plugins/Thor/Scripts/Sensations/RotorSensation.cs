using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rotor", menuName = "Sensations/Rotor")]
public class RotorSensation : SpeedConstrainableSensation
{
    public float length = 0.07f;
    public float rotationFrequency = 1;

    private double rotationFraction = 0;

    public override bool IsClosedShape { get { return false; } }

    public bool playLineBackAndForth = true;

    public override (Vector3 p, float intensity) EvaluateAt(double seconds)
    {
        rotationFraction = GetFraction(seconds, rotationFrequency);
        return (Lerp(GetFraction(seconds)), 1);
    }

    public override Vector3 Lerp(double fraction)
    {
        Vector3 start = new Vector3(-length / 2, 0, 0);
        Vector3 end = new Vector3(length / 2, 0, 0);

        Quaternion lineRotation = Quaternion.Euler(new Vector3(0, (float)rotationFraction * 360, 0));

        start = lineRotation * start;
        end = lineRotation * end;

        if (playLineBackAndForth)
        {
            fraction = fraction <= 0.5 ? fraction * 2 : 1 - ((fraction - 0.5) * 2);
        }

        return Shape.LineAt(fraction, start, end);
    }

    protected override float ComputeFrequency()
    {
        return speedTravel / (length * (playLineBackAndForth ? 2 : 1));
    }
}
