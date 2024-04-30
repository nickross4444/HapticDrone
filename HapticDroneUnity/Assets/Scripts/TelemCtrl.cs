using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelemCtrl : MonoBehaviour
{
    public Telemetry telem;
    //public float speed;
    //public float rotationSpeed = 60;
    //[HideInInspector]
    public Vector3 cmdPos = new Vector3(0, 1, 0);      //start at 1 meter
    public float maxHeight;
    [HideInInspector]
    public float cmdRot = 0;       //degrees
    // Start is called before the first frame update
    void Start()
    {
        telem.eStopOut = 0;
    }
    void Update()
    {
        if (cmdPos.y > maxHeight)
            cmdPos.y = maxHeight;
        //update output telemetry with desired position:
        Vector3 translatedPos = unityToDroneCoordinates(cmdPos);
        telem.xOut = translatedPos.x;
        telem.yOut = translatedPos.y;
        telem.zOut = translatedPos.z;
        telem.rOut = cmdRot * Mathf.PI / 180f;      //degrees to radians

        if (Input.GetKeyDown(KeyCode.A)) telem.armOUT = telem.armOUT > 0 ? 0 : 1;
        if (Input.GetKeyDown(KeyCode.L)) telem.takeoffOUT = telem.takeoffOUT > 0 ? 0 : 1;
    }
    void FixedUpdate()
    {
        //move virtual drone to real drone pos:
        Vector3 telemPos = new Vector3(telem.x, telem.y, telem.z);
        gameObject.transform.position = droneToUnityCoordinates(telemPos);
        Vector3 telemRot = new Vector3(telem.pitch, telem.yaw, telem.roll);
        gameObject.transform.eulerAngles = droneToUnityAngles(telemRot);

        
        //This is temporary while we don't have hanptic feedback
        #region keycontrol
        /*
        if (Input.GetKey(KeyCode.W)) cmdPos.z += Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.S)) cmdPos.z -= Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.A)) cmdPos.x -= Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.D)) cmdPos.x += Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.E)) cmdRot -= Time.deltaTime * rotationSpeed;
        if (Input.GetKey(KeyCode.Q)) cmdRot += Time.deltaTime * rotationSpeed;
        if (Input.GetKey(KeyCode.Space)) cmdPos.y += Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.LeftShift)) cmdPos.y -= Time.deltaTime * speed;
        */
        #endregion
    }
    Vector3 droneToUnityCoordinates(Vector3 pos)
    {
        return new Vector3(-pos.y, pos.z, pos.x);
    }
    Vector3 unityToDroneCoordinates(Vector3 pos)
    {
        return new Vector3(pos.z, -pos.x, pos.y);
    }
    Vector3 droneToUnityAngles(Vector3 rot)
    {
        rot.y = -rot.y;
        rot.z = -rot.z;
        return rot*360f/2/Mathf.PI;
    }
}
