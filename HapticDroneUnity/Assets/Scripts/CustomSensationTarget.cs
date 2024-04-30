using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;


public class CustomSensationTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public HandModelBase handToTrack;
    public GameObject drone;
    public DeviceManager device;
    public GameObject childPlane;
    public Vector3 offset;

    void Start()
    {
        childPlane.transform.localPosition += offset;    //this transforms the offset to the local coordinate system
    }
    private void FixedUpdate()
    {
        Hand _hand = handToTrack.GetLeapHand();
        if (_hand != null)
            device.StartEmitter();
        else
        {
            if (device.GetSensation() != null)
                device.GetSensation().RestoreTransform();
            device.StopEmitter();
            return;
        }
        transform.position = _hand.PalmPosition;
        transform.rotation = _hand.Rotation;
    }
    void Update()
    {
        //childPlane.transform.localEulerAngles = new Vector3(0, drone.transform.eulerAngles.y, 0);       //rotate sensation with the drone
        if (device.GetSensation() != null)
        {
            device.GetSensation().position = childPlane.transform.position;
            device.GetSensation().rotation = childPlane.transform.rotation.eulerAngles;// + new Vector3(0, drone.transform.eulerAngles.y, 0) ;
        }
    }
}
