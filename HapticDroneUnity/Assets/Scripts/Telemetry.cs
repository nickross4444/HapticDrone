using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Telemetry : MonoBehaviour
{
    //input vars:
    public float[] inputs = new float[27];
    public float time                           { get { return inputs[00]; } set { inputs[00] = value; } }
    public float x                              { get { return inputs[01]; } set { inputs[01] = value; } }
    public float y                              { get { return inputs[02]; } set { inputs[02] = value; } }
    public float z                              { get { return inputs[03]; } set { inputs[03] = value; } }
    public float roll                           { get { return inputs[04]; } set { inputs[04] = value; } }
    public float pitch                          { get { return inputs[05]; } set { inputs[05] = value; } }
    public float yaw                            { get { return inputs[06]; } set { inputs[06] = value; } }
    public float state                          { get { return inputs[07]; } set { inputs[07] = value; } }
    public float initializing                   { get { return inputs[08]; } set { inputs[08] = value; } }
    public float commIssue                      { get { return inputs[09]; } set { inputs[09] = value; } }
    public float trackingIssue                  { get { return inputs[10]; } set { inputs[10] = value; } }
    public float lowBattery                     { get { return inputs[11]; } set { inputs[11] = value; } }
    public float armState                       { get { return inputs[12]; } set { inputs[12] = value; } }
    public float takeoffState                   { get { return inputs[13]; } set { inputs[13] = value; } }
    public float eStop                          { get { return inputs[14]; } set { inputs[14] = value; } }
    public float joystickIssue                  { get { return inputs[15]; } set { inputs[15] = value; } }
    public float cmdThrottleAtTrim              { get { return inputs[16]; } set { inputs[16] = value; } }
    public float cmdThrottleCloseToZero         { get { return inputs[17]; } set { inputs[17] = value; } }
    public float controllerThrottleClostToZero  { get { return inputs[18]; } set { inputs[18] = value; } }
    public float takeoffSuccessful              { get { return inputs[19]; } set { inputs[19] = value; } }
    public float flyingToLow                    { get { return inputs[20]; } set { inputs[20] = value; } }
    public float flyingToHigh                   { get { return inputs[21]; } set { inputs[21] = value; } }
    public float closeToGround                  { get { return inputs[22]; } set { inputs[22] = value; } }
    public float sensorFail                     { get { return inputs[23]; } set { inputs[23] = value; } }
    public float nextState                      { get { return inputs[24]; } set { inputs[24] = value; } }
    public float error                          { get { return inputs[25]; } set { inputs[25] = value; } }
    public float stopModel                      { get { return inputs[26]; } set { inputs[26] = value; } }

    //output vars:
    public float[] outputs = new float[08];
    public float timeOut                        { get { return outputs[00]; } set { outputs[00] = value; } }
    public float armOUT                         { get { return outputs[01]; } set { outputs[01] = value; } }
    public float takeoffOUT                     { get { return outputs[02]; } set { outputs[02] = value; } }
    public float eStopOut                       { get { return outputs[03]; } set { outputs[03] = value; } }
    public float xOut                           { get { return outputs[04]; } set { outputs[04] = value; } }
    public float yOut                           { get { return outputs[05]; } set { outputs[05] = value; } }
    public float zOut                           { get { return outputs[06]; } set { outputs[06] = value; } }
    public float rOut                           { get { return outputs[07]; } set { outputs[07] = value; } }

    private void Update()
    {
        timeOut = time;
    }
}
