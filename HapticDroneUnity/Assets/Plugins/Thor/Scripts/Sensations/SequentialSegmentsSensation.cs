using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "SequentialSegments", menuName = "Sensations/SequentialSegments")]
public class SequentialSegmentsSensation : OpenedShapeSensation
{
    [SerializeField]
    public List<Vector3> segments;

    public float playbackFrequency = 1;

    private Segment currentSegment;

    public bool playLineBackAndForth = true;

    public override (Vector3 p, float intensity) EvaluateAt(double seconds)
    {
        if (segments.Count == 0)
        {
            return (Vector3.zero, 0);
        }

        double segmentFraction = GetFraction(seconds, playbackFrequency);

        // Calculate on which segment we are
        int segmentIndex = (int)Mathf.Floor((float)segmentFraction * segments.Count);

        Vector3 start = segments.ElementAt(segmentIndex);
        Vector3 end = segmentIndex + 1 >= segments.Count ? segments.ElementAt(0) : segments.ElementAt(segmentIndex + 1);

        currentSegment = new Segment(start, end);

        return (Lerp(GetFraction(seconds)), 1);
    }

    public override Vector3 Lerp(double fraction)
    {
        if (playLineBackAndForth)
        {
            fraction = fraction <= 0.5 ? fraction * 2 : 1 - ((fraction - 0.5) * 2);
        }

        return Shape.LineAt(fraction, currentSegment.start, currentSegment.end);
    }
}
