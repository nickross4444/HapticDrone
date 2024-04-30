using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensationTarget : MonoBehaviour
{
    public DeviceManager device;

    void Update()
    {
        device.GetSensation().position = transform.position;
        device.GetSensation().rotation = transform.rotation.eulerAngles;
    }

    private void OnDisable()
    {
        device.GetSensation().RestoreTransform();
    }
}
