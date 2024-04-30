using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shape
{
    public static Vector3 CircleAt(double fraction, float radius, float phi = 0)
    {
        return new Vector3(
            (float)(Math.Cos(2.0 * Math.PI * fraction + phi) * radius),
            0,
            (float)(Math.Sin(2.0 * Math.PI * fraction + phi) * radius)
        );
    }

    public static Vector3 CircleAt(double fraction, float radius, Vector3 center, float phi = 0)
    {
        return CircleAt(fraction, radius, phi) + center;
    }

    public static Vector3 LineAt(double fraction, Vector3 start, Vector3 end)
    {
        return Vector3.Lerp(start, end, (float)fraction);
    }

    public static Vector3 CosineLineAt(double fraction, Vector3 center, Vector3 direction)
    {
        return center + (float)Math.Cos(fraction * 2 * Math.PI) * (direction / 2);
    }

    public static Vector3 LissajousAt(double fraction, float a, float b, float delta, float width, float height)
    {
        // x = A * sin(a * t + delta)
        // y = B * sin(b * t)
        // For a circle, A = B = radius and a = b, and delta = pi / 2
        // For a line, A = B = radius, a = b, and delta = 0
        return new Vector3(
            (float)(Math.Sin(a * (fraction * 2 * Math.PI) + delta) * width),
            0,
            (float)(Math.Sin(b * (fraction * 2 * Math.PI)) * height)
        );
    }
}
