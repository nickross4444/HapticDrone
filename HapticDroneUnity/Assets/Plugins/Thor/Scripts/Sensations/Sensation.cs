using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

abstract public class Sensation : ScriptableObject, ISensation
{
    [Header("Transform")]
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale = Vector3.one;

    private Matrix4x4 originMatrix;

    [Header("Sensation parameters")]
    [SerializeField]
    protected float frequency = 80;

    protected virtual float Frequency
    {
        get
        {
            return frequency;
        }
        set
        {
            frequency = value;
        }
    }

    abstract public (Vector3 p, float intensity) EvaluateAt(double seconds);

    abstract public Vector3 Lerp(double fraction);

    /// <summary>
    /// Indicate if the sensation is a closed shape (for example a circle). Mainly used to draw the sensation.
    /// It defaults to true and must be reimplemented by sensations that wish to be drawn as an open shaped (not connecting the last and first point)
    /// </summary>
    public virtual bool IsClosedShape { get { return true; } }

    public virtual List<Vector3> GetLinePoints(int number)
    {
        return Enumerable.Range(0, number)
            .Select(sample => Transform(Lerp(sample / (number * 1.0))))
            .ToList();
    }

    public Vector3 Transform(Vector3 point)
    {
        Matrix4x4 test = Matrix4x4.TRS(position, Quaternion.Euler(rotation), scale);
        point = test.MultiplyPoint3x4(point);
        return point;
    }

    public (Vector3 p, float intensity) EvaluateCPAt(double seconds)
    {
        (Vector3 p, float intensity) = EvaluateAt(seconds);
        p = Transform(p);
        return (p, intensity);
    }

    public Vector3 PointAt(double seconds)
    {
        Vector3 point = Lerp(seconds);
        point = Transform(point);
        return point;
    }

    public void SaveTransform()
    {
        originMatrix = Matrix4x4.TRS(position, Quaternion.Euler(rotation), scale);
    }

    public void RestoreTransform()
    {
        // Extract new local position
         position = originMatrix.GetColumn(3);

        // Extract new local rotation
        rotation = Quaternion.LookRotation(
            originMatrix.GetColumn(2),
            originMatrix.GetColumn(1)
        ).eulerAngles;

        // Extract new local scale
        scale = new Vector3(
            originMatrix.GetColumn(0).magnitude,
            originMatrix.GetColumn(1).magnitude,
            originMatrix.GetColumn(2).magnitude
        );
    }

    /// <summary>
    /// Returns a fraction between 0 and 1 depending on the frequency of which the sensation
    /// should be played. All sensations uses it.
    /// </summary>
    /// <param name="seconds">The time at which the fraction should be computed</param>
    protected double GetFraction(double seconds)
    {
        return GetFraction(seconds, Frequency);
    }

    /// <summary>
    /// Returns a fraction between 0 and 1 depending on a given frequency. Useful for calculating
    /// things on a scale from 0 to 1.
    /// </summary>
    /// <param name="seconds">The time at which the fraction should be computed</param>
    /// <param name="frequency">The frequency at which we want to find the fraction</param>
    /// <returns></returns>
    protected double GetFraction(double seconds, float frequency)
    {
        double fraction = seconds * frequency;
        fraction -= (long)fraction;
        return fraction;
    }
}


[Serializable]
public enum FrequencyRule
{
    Fixed,
    DependOnSpeedTravel
}


abstract public class SpeedConstrainableSensation : Sensation
{
    public float speedTravel = 8;
    public FrequencyRule frequencyRule = FrequencyRule.Fixed;

    protected override float Frequency
    {
        get
        {
            if (frequencyRule == FrequencyRule.DependOnSpeedTravel)
            {
                frequency = ComputeFrequency();
            }
            return base.Frequency;
        }
        set
        {
            frequency = value;
        }
    }

    abstract protected float ComputeFrequency();
}


/// <summary>
/// If you want you can derive from this class if you don't want to reimplement IsClosedShape
/// </summary>
abstract public class OpenedShapeSensation : Sensation
{
    abstract public override (Vector3 p, float intensity) EvaluateAt(double seconds);

    abstract public override Vector3 Lerp(double fraction);

    public override bool IsClosedShape { get { return false; } }
}