using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LockedGrid", menuName = "Sensations/LockedGrid")]
public class LockedGrid : Sensation, ISensation
{
    public float radius;
    public float gridSize;
    [SerializeField]
    public List<Vector3> centers;
    List<Vector3> gridCenters;      //centers modified to snap to a grid
    //int count;
    public LockedGrid()
    {
        //count = centers.Count;
        //gridCenters = new List<Vector3>(centers);       //clone into new list
    }
    public override bool IsClosedShape { get { return false; } }

    public override (Vector3 p, float intensity) EvaluateAt(double seconds)
    {
        updateGrid();
        //fraction loops 0-1 at frequency
        double frac = GetFraction(seconds);// / centers.Count);      //slow counter down so each circle is drawn at normal speed
        Vector3 p = Lerp(frac);    
        return (p, 1);
    }

    public override Vector3 Lerp(double fraction)
    {
        // Calculate on which cirlce we are
        int cirlceIndex = (int)Mathf.Floor((float)fraction * gridCenters.Count);
        // Calculate how far we are in the segment between 0 and 1
        float minFraction = cirlceIndex / (float)gridCenters.Count;
        float maxFraction = (cirlceIndex + 1) / (float)gridCenters.Count;
        fraction = Mathf.InverseLerp(minFraction, maxFraction, (float)fraction);
        if (cirlceIndex < 0 || cirlceIndex >= gridCenters.Count)        //return if out of range, fixes error in console during editor time
            return new Vector3(0,0,0);
        return Shape.CircleAt(fraction, radius, gridCenters[cirlceIndex]);
    }
    void updateGrid()
    {
        gridCenters = new List<Vector3>(centers);       //clone into new list
        for (int i = 0; i < gridCenters.Count; i++)
        {
            Vector3 oldVal = gridCenters[i];
            //float newX = oldVal.x - (oldVal.x % gridSize);
            //float newZ = oldVal.z - (oldVal.z % gridSize);
            float newX = oldVal.x - ( (position.x/scale.x) % gridSize);
            float newZ = oldVal.z - ( (position.z/scale.z) % gridSize);
            gridCenters[i] = new Vector3(newX, oldVal.y, newZ);  
        }
    }

    //protected override float ComputeFrequency()
    //{
    //    return speedTravel / (2 * Mathf.PI * radius);
    //}
}
