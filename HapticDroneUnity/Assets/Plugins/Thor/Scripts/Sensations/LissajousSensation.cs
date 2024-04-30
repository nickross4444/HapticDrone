using UnityEngine;

[CreateAssetMenu(fileName = "Lissajous", menuName = "Sensations/Lissajous")]
public class LissajousSensation : Sensation
{
    public float a;
    public float b;

    public float delta;

    public float width;
    public float height;

    public override (Vector3 p, float intensity) EvaluateAt(double seconds)
    {
        Vector3 p = Lerp(GetFraction(seconds));
        return (p, 1);
    }

    public override Vector3 Lerp(double fraction)
    {
        return Shape.LissajousAt(fraction, a, b, delta, width, height);
    }
}
