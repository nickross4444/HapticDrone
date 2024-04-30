using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomMulti", menuName = "Sensations/CustomMulti")]
public class CustomMulti : Sensation, ISensation
{
    [SerializeField]
    public List<Vector3> segments;
    [SerializeField]
    [Range(0.0f, 1f)]
    float drawRatio = 1;
    [SerializeField]
    [Range(0.01f, 2f)]
    float drawScale = 1f;
    float initialScale;
    public override bool IsClosedShape { get { return false; } }
    public override (Vector3 p, float intensity) EvaluateAt(double seconds)
    {
        return (Lerp( GetFraction(seconds)), 1);
    }

    public override Vector3 Lerp(double fraction)
    {
        if (segments.Count == 0)
        {
            return Vector3.zero;
        }

        // Calculate on which segment we are
        int segmentIndex = (int)Mathf.Floor((float)fraction * segments.Count);

        // Calculate how far we are in the segment between 0 and 1
        float minFraction = segmentIndex / (float)segments.Count;
        float maxFraction = (segmentIndex + 1) / (float)segments.Count;
        fraction = Mathf.InverseLerp(minFraction, maxFraction, (float)fraction);

        Vector3 start = segments.ElementAt(segmentIndex);
        Vector3 end = segmentIndex + 1 >= segments.Count ? segments.ElementAt(0) : segments.ElementAt(segmentIndex + 1);
        end = start + drawRatio * (end - start);

        return Shape.LineAt(fraction, drawScale * start, drawScale * end);
    }
}
