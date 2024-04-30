using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Scan", menuName = "Sensations/Scan")]
public class ScanSensation : SpeedConstrainableSensation
{
    public float scanFrequency = 1.0f;
    public float length = 0.07f;

    private double scanFraction;

    public ScanStyle scanStyle = ScanStyle.Loop;
    public ScanDirection scanDirection = ScanDirection.TowardFingertips;

    public override bool IsClosedShape { get { return false; } }

    public bool playLineBackAndForth = true;

    public override (Vector3 p, float intensity) EvaluateAt(double seconds)
    {
        if (scanStyle == ScanStyle.BackAndForth)
        {
            // The backAndForthFraction is used to calculate the direction of the scan when switching back and forth
            // if it's over 0.5, it means we are on the second part of the back and forth process
            double backAndForthFraction = GetFraction(seconds, scanFrequency * 0.5f);
            scanDirection = backAndForthFraction >= 0.5 ? ScanDirection.TowardWrist : ScanDirection.TowardFingertips;
        }

        // The scanFraction is how far we are in the animation, between 0 and 1
        scanFraction = GetFraction(seconds, scanFrequency);

        Vector3 p = Lerp(GetFraction(seconds));
        return (p, 1);
    }

    public override Vector3 Lerp(double fraction)
    {
        float z = GetZ();

        Vector3 start = new Vector3(-length / 2, 0, z);
        Vector3 stop = new Vector3(length / 2, 0, z);

        if (playLineBackAndForth)
        {
            fraction = fraction <= 0.5 ? fraction * 2 : 1 - ((fraction - 0.5) * 2);
        }

        return Shape.LineAt(fraction, start, stop);
    }

    public override List<Vector3> GetLinePoints(int number)
    {
        float z = GetZ();

        return Enumerable.Range(0, number)
            .Select(sample => {
                return Transform(Shape.LineAt(
                    sample / (float)number,
                    new Vector3(-length / 2, 0, z),
                    new Vector3(length / 2, 0, z)
                ));
            })
            .ToList();
    }

    private float GetZ()
    {
        double directedScanFraction = scanDirection == ScanDirection.TowardFingertips ? scanFraction : 1 - scanFraction;
        return (float)((length * directedScanFraction) - (length / 2));
    }

    protected override float ComputeFrequency()
    {
        return speedTravel / (length * (playLineBackAndForth ? 2 : 1));
    }
}

public enum ScanStyle
{
    Loop,
    BackAndForth
}

public enum ScanDirection
{
    TowardFingertips,
    TowardWrist
}
