using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class DrawSensation : MonoBehaviour
{
    public DeviceManager device;

    private LineRenderer lr;
    public int samples = 60;

    [Header("Line settings")]
    public Material lineMaterial;
    public float lineWidth = 0.005f;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        SetLineParameters();

        if (device == null)
        {
            device = GetComponent<DeviceManager>();
        }
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.update += EditorUpdate;
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        // In Editor mode, we add this update function to the update thread
        // it is not done in Play mode as this would mess the rendering
        EditorApplication.update -= EditorUpdate;
#endif
    }

#if UNITY_EDITOR
    /// <summary>
    /// An update function that is called only in Editor mode
    /// It invokes EvaluateAt() of the Sensation to update sensations that are dependant on time (scan for example)
    /// then DrawLine() to draw the line as it would do in any case
    /// </summary>
    public void EditorUpdate()
    {
        if (!Application.isPlaying)
        {
            device?.GetSensation()?.EvaluateAt(Time.time);
            DrawLine();
        }
    }
#endif

    void LateUpdate()
    {
        DrawLine();
    }

    private void SetLineParameters()
    {
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;

        lr.useWorldSpace = true;
        lr.positionCount = samples;
        lr.loop = device?.GetSensation()?.IsClosedShape ?? true;

        lr.material = lineMaterial;
    }

    private void DrawLine()
    {
        if (device == null || lr == null)
        {
            return;
        }

        List<Vector3> pos = device.GetSensation()?.GetLinePoints(samples) ?? new List<Vector3>() { Vector3.zero };
        lr.loop = device.GetSensation()?.IsClosedShape ?? false;
        lr.positionCount = pos.Count;
        lr.SetPositions(pos.ToArray());
    }

    private void OnValidate()
    {
        // Update the line renderer
        if (lr == null)
        {
            lr = GetComponent<LineRenderer>();
        }
        SetLineParameters();
    }
}
