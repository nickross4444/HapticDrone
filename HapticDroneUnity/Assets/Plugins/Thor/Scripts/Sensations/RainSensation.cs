using UnityEngine;

[CreateAssetMenu(fileName = "Rain", menuName = "Sensations/RainSensation")]
public class RainSensation : SpeedConstrainableSensation
{
    public double rainSpeed;
    public float areaRadiusMax;
    public float radius;

    private Vector3 offset;
    private double previousTime = 0.0;

    System.Random random;

    public override (Vector3 p, float intensity) EvaluateAt(double seconds)
    {
        if (random == null)
        {
            random = new System.Random();
        }

        if (previousTime < 0.0)
        {
            previousTime = 0.0;
        }

        if (previousTime > seconds)
        {
            previousTime = seconds;
        }

        if ((seconds - previousTime) > rainSpeed)
        {
            double rand_theta = random.NextDouble() * 2 * Mathf.PI;
            double rand_r = random.NextDouble() * areaRadiusMax;

            offset = new Vector3(
                (float)rand_r * Mathf.Cos((float)rand_theta),
                0.0f,
                (float)rand_r * Mathf.Sin((float)rand_theta)
            );

            previousTime = seconds;
        }

        Vector3 p = Lerp(GetFraction(seconds));
        return (p, 1);
    }

    public override Vector3 Lerp(double fraction)
    {
        return Shape.CircleAt(fraction, radius) + offset;
    }

    protected override float ComputeFrequency()
    {
        return speedTravel / (2 * Mathf.PI * radius);
    }
}
