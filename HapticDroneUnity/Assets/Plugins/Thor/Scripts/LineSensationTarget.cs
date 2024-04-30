using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LineSensationTarget : MonoBehaviour
{
    public DeviceManager device;

    public Transform start;
    public Transform stop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Sensation sensation = device?.GetSensation();

        // Yes this is a line we can mess with it
        if (sensation != null && sensation.GetType() == typeof(LineSensation))
        {
            LineSensation lineSensation = (LineSensation)sensation;
            lineSensation.start = start.position;
            lineSensation.stop = stop.position;
            lineSensation.position = Vector3.zero;
        }
    }
}
