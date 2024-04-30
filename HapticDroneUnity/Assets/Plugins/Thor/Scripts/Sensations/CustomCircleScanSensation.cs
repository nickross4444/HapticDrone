using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomCircleScan", menuName = "Sensations/CustomCircleScan")]
public class CustomCircleScanSensation : Sensation
{
    public float scanFrequency = 1;
    public float minRadius = 0.01f;
    public float maxRadius = 0.03f;
    public float defaultRadius = 0.02f;
    public Vector3 lockPos = new Vector3(0,0,0);
    public bool scanning = false;

    public CircleScanStyle scanStyle = CircleScanStyle.OpeningOrClosing;
    public CircleScanDirection scanDirection = CircleScanDirection.Opening;

    private double scanFraction;

    public override (Vector3 p, float intensity) EvaluateAt(double seconds)
    {
        scanFraction = GetFraction(seconds, scanFrequency);
        return (Lerp(GetFraction(seconds)), 1);
    }

    public override Vector3 Lerp(double fraction)
    {

        float radius;
        if (scanning)
        {
            radius =
                scanDirection == CircleScanDirection.Opening
                ? Mathf.Lerp(minRadius, maxRadius, (float)scanFraction)
                : Mathf.Lerp(maxRadius, minRadius, (float)scanFraction);
        } else {
            radius = defaultRadius;
        }
        Vector3 center = new Vector3(lockPos.x, 0, lockPos.z);
        return Shape.CircleAt(fraction, radius, center);
    }
}
//public enum CircleScanDirection
//{
//    Opening,
//    Closing
//}

//public enum CircleScanStyle
//{
//    OpeningOrClosing,
//    Breathing
//}
