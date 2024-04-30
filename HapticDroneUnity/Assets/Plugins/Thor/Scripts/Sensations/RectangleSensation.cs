using UnityEngine;

[CreateAssetMenu(fileName = "Rectangle", menuName = "Sensations/Rectangle")]
public class RectangleSensation : SpeedConstrainableSensation
{
    public float width;
    public float height;

    public override (Vector3 p, float intensity) EvaluateAt(double seconds)
    {
        return (Lerp(GetFraction(seconds)), 1);
    }

    public override Vector3 Lerp(double fraction)
    {
        Vector3 start;
        Vector3 stop;

        if (fraction <= 0.25)
        {
            start = new Vector3(-width / 2, 0, -height / 2);
            stop = new Vector3(-width / 2, 0, height / 2);

            fraction = Mathf.InverseLerp(0, 0.25f, (float)fraction);
        }
        else if (fraction > 0.25 && fraction <= 0.5)
        {
            start = new Vector3(-width / 2, 0, height / 2);
            stop = new Vector3(width / 2, 0, height / 2);

            fraction = Mathf.InverseLerp(0.25f, 0.5f, (float)fraction);
        }
        else if (fraction > 0.5 && fraction <= 0.75)
        {
            start = new Vector3(width / 2, 0, height / 2);
            stop = new Vector3(width / 2, 0, -height / 2);

            fraction = Mathf.InverseLerp(0.5f, 0.75f, (float)fraction);
        }
        else
        {
            start = new Vector3(width / 2, 0, -height / 2);
            stop = new Vector3(-width / 2, 0, -height / 2);

            fraction = Mathf.InverseLerp(0.75f, 1, (float)fraction);
        }

        return Shape.LineAt(fraction, start, stop);

    }

    protected override float ComputeFrequency()
    {
        return speedTravel / ((2 * width) + (2 * height));
    }
}
