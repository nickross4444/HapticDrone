using System;
using UnityEngine;
using Ultraleap.Haptics;

public class DeviceManager : MonoBehaviour
{
    StreamingEmitter _emitter;
    DateTime _startTime;

    [SerializeField]
    private Sensation sensation;

    [Range(0, 1)]
    public float maxIntensity;

    [Range(0, 1)]
    public float minIntensity;

    private Matrix4x4 m4x4;

#if UNITY_EDITOR
    private Sensation previousSensation;
#endif

    [Header("Active zone")]
    public Bounds bounds = new Bounds(new Vector3(0, 0.15f, 0), new Vector3(0.2f, 0.3f, 0.2f));
    public bool showZone = false;

    [Header("Leap alignment")]
    public bool autoSetAlignment = true;
    public UnityEngine.Transform leapTransform;

    [Header("Array rotation")]
    public bool setRotation = true;
    public Vector3 rotation;


    void Awake()
    {

#if UNITY_EDITOR
        previousSensation = sensation;
#endif
        sensation?.SaveTransform();

        Library lib = new Library();
        try
        {
            lib.Connect();
        }
        catch
        {
            Debug.LogError("Couldn't connect to daemon");
            return;
        }

        _startTime = DateTime.Now;

        IDevice device = lib.FindDevice();
        Debug.Log($"Device found : {device.Info.Identifier}");
        Debug.Log($"Model : {device.Info.ModelName}");
        Debug.Log($"Description : {device.Info.ModelDescription}");
        Debug.Log($"Serial Nr : {device.Info.SerialNumber}");
        Debug.Log($"Firmware version : {device.Info.FirmwareVersion}");

        if (leapTransform != null && autoSetAlignment)
        {
            System.Numerics.Vector3 leapPosition = device.GetKitTransform().Origin;
            leapTransform.position = new Vector3(leapPosition.X, leapPosition.Z, leapPosition.Y);
        }

        if (setRotation)
        {
            transform.rotation = Quaternion.Euler(rotation);
        }

        _emitter = new StreamingEmitter(lib);
        _emitter.Devices.Add(device, new Ultraleap.Haptics.Transform());
        _emitter.SetControlPointCount(1, AdjustRate.AsRequired);
        _emitter.EmissionCallback = Callback;
        _emitter.Start();
    }

    public Sensation GetSensation()
    {
        return sensation;
    }

    public void SetSensation(Sensation newSensation)
    {
        // Restore previous if needed
        sensation?.RestoreTransform();

        // Assign new sensation
        sensation = newSensation;

        // Save new just in case someone plays with the restoreTransformOnStop flag,
        // it will still restore with the good values
        sensation?.SaveTransform();
    }


    private void FixedUpdate()
    {
        m4x4 = transform.localToWorldMatrix.inverse;
    }

    private void Callback(StreamingEmitter emitter, StreamingEmitter.Interval interval, DateTimeOffset submissionDeadline)
    {
        foreach (var sample in interval)
        {
            if (sensation == null)
            {
                sample.Points[0].Intensity = 0;
                continue;
            }

            double seconds = (sample.Time - _startTime).TotalSeconds;
            (Vector3 p, float intensity) = sensation.EvaluateCPAt(seconds);

            p = m4x4.MultiplyPoint3x4(p);
            bool isInside = bounds.Contains(p);
            p = Thor.Utils.FromUnitySpaceToThorSpace(p);

            sample.Points[0].Position = new System.Numerics.Vector3(p.x, p.y, p.z);
            sample.Points[0].Intensity = isInside ? Mathf.Lerp(minIntensity, maxIntensity, intensity) : 0;
        }
    }

    // Ensure the emitter is immediately disposed when destroyed
    void OnDestroy()
    {
        sensation?.RestoreTransform();
        _emitter?.Stop();
        _emitter.Devices.Clear();
        _emitter = null;
    }

    private void OnDisable()
    {
        _emitter?.Stop();
    }

    private void OnEnable()
    {
        _emitter?.Start();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        minIntensity = Mathf.Min(minIntensity, maxIntensity);
        maxIntensity = Mathf.Max(maxIntensity, minIntensity);

        // Restore previous if needed
        previousSensation?.RestoreTransform();

        // Save new just in case someone plays with the restoreTransformOnStop flag,
        // it will still restore with the good values
        sensation?.SaveTransform();

        if (setRotation)
        {
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
#endif

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (showZone)
        {
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }

    public void StartEmitter()
    {
        _emitter.Start();
    }

    public void StopEmitter()
    {
        _emitter.Stop();
    }
}

namespace Thor
{
    public class Utils
    {
        public static Vector3 FromUnitySpaceToThorSpace(Vector3 point)
        {
            return new Vector3(point.x, point.z, point.y);
        }

        public static Vector3 FromThorSpaceToUnitySpace(Vector3 point)
        {
            return new Vector3(point.x, point.z, point.y);
        }
    }
}
