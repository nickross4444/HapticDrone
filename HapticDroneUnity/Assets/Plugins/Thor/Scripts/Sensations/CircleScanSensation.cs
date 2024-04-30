using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CircleScan", menuName = "Sensations/CircleScan")]
public class CircleScanSensation : Sensation
{
    public float scanFrequency = 1;
    public float minRadius = 0.01f;
    public float maxRadius = 0.03f;

    public CircleScanStyle scanStyle = CircleScanStyle.OpeningOrClosing;
    public CircleScanDirection scanDirection = CircleScanDirection.Opening;

    private double scanFraction;

    public override (Vector3 p, float intensity) EvaluateAt(double seconds)
    {
        if (scanStyle == CircleScanStyle.Breathing)
        {
            // The backAndForthFraction is used to calculate the direction of the scan when switching back and forth
            // if it's over 0.5, it means we are on the second part of the back and forth process
            double backAndForthFraction = GetFraction(seconds, scanFrequency * 0.5f);
            scanDirection = backAndForthFraction >= 0.5 ? CircleScanDirection.Closing : CircleScanDirection.Opening;
        }

        scanFraction = GetFraction(seconds, scanFrequency);
        return (Lerp(GetFraction(seconds)), 1);
    }

    public override Vector3 Lerp(double fraction)
    {
        float radius = 
                scanDirection == CircleScanDirection.Opening
                ? Mathf.Lerp(minRadius, maxRadius, (float)scanFraction)
                : Mathf.Lerp(maxRadius, minRadius, (float)scanFraction);
        return Shape.CircleAt(fraction, radius);
    }
}

public enum CircleScanDirection
{
    Opening,
    Closing
}

public enum CircleScanStyle
{
    OpeningOrClosing,
    Breathing
}
