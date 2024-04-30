using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using UnityEngine.UI;

public class StateController : MonoBehaviour
{
    public enum controlMethods
    {
        PositionControl,
        JoystickControl
    }
    public controlMethods controlMethod;
    public float landHeightTrigger = 0.2f;
    public float positionCtrlScale = 5f;
    public float joystickCtrlScale = 10f;
    public float rotationSpeed = 1f;
    public float joyStickDeadZone = 0.1f;
    public float joyStickHeightHapticDeadZone = 0.5f;
    float rotationThreshPos = 0;       //centered around -10 degrees for right hand
    float rotationThreshNeg = 340;
    public TMPro.TMP_Dropdown UIControlModeDropdown;
    public Toggle armTog, fistTog, takeoffTog;
    public Button backBtn;
    public Telemetry telem;
    public DeviceManager hapticDevice;
    public HandModelBase controlHand;
    public Sensation armed;
    public Sensation takeoff;
    public Sensation landing;
    public Sensation posCtrlSens;
    public CustomCircleScanSensation joyCtrlSens;
    TelemCtrl telemetryController;

    public void updateMode()
    {
        if (UIControlModeDropdown.value == 0)
            controlMethod = controlMethods.PositionControl;
        else
            controlMethod = controlMethods.JoystickControl;
    }
    void Start()
    {
        telemetryController = FindObjectOfType<TelemCtrl>();
    }
    bool closedHandSeen = false;
    bool hasTakenOff = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))            //ability to toggle mode mid-flight
        {
            if (UIControlModeDropdown.value == 0)
                UIControlModeDropdown.value = 1;
            else
                UIControlModeDropdown.value = 0;
            updateMode();
        }
        updateToggles();
        Hand hand = controlHand.GetLeapHand();
        if (hand == null)
        {
            closedHandSeen = false;
            return;
        }
        if (handOpen(hand) && !closedHandSeen)
        {
            hapticDevice.StopEmitter();
            return;      //don't do anything if closed hand has not been seen
        } else {
            hapticDevice.StartEmitter();
        }
        if (!handOpen(hand)) closedHandSeen = true;       //say we've seen a closed hand
        switch ((int)telem.state)
        {
            ///States:
            ///1: idle
            ///2: armed
            ///3: takeoff throttle
            ///4: takeoff height
            ///5: flight
            ///6: autoland
            ///7: Emergency land
            ///8: Emergency abort
            ///9: stop
            case 0:
            case 1:     //do nothing
                hapticDevice.SetSensation(null);
                hapticDevice.StopEmitter();
                break;
            case 2:     //light buzz to indicate armed
                if (hapticDevice.GetSensation() != armed)
                    hapticDevice.SetSensation(armed);
                if (handOpen(hand))       //takeoff if the control hand has the thum extended
                {
                    telem.takeoffOUT = 1f;
                    updateDynamicOffset(hand);      //ensure that the offset is set at takeoff
                }
                break;
            case 3:     //move to case 4; don't break
            case 4:
                if (hapticDevice.GetSensation() != takeoff)
                    hapticDevice.SetSensation(takeoff);
                break;
            case 5:
                if (controlMethod == controlMethods.JoystickControl)
                {
                    joystickCtrlHaptics(hand);
                    joystickControl(hand);
                }
                else if (controlMethod == controlMethods.PositionControl)
                {
                    positionCtrlHaptics(hand);
                    positionControl(hand);
                }
                if (!hasTakenOff)       //if still on the ground
                    hasTakenOff = !checkForLand();
                else
                    checkForLand();     //if flying
                    break;
            case 6:
                if (hapticDevice.GetSensation() != landing)
                    hapticDevice.SetSensation(landing);
                break;
            case int i when i > 6:      //all other cases
            default:
                hapticDevice.SetSensation(null);
                hapticDevice.StopEmitter();
                break;
        }
    }
    Vector3 initialDronePos;
    Vector3 initialHandPos;
    void updateDynamicOffset(Hand h) 
    {
        initialDronePos = telemetryController.cmdPos;
        if (h != null)
            initialHandPos = h.PalmPosition;// * positionCtrlScale;
    }
    void joystickControl(Hand h)
    {
        if (!handOpen(h))       //if hand is closed, update offset
        {
            updateDynamicOffset(h);
            return;
        }
        Vector3 handDelta = (h.PalmPosition - initialHandPos) * joystickCtrlScale;
        Vector3 timeCorrectedDelta = handDelta * Time.deltaTime;
        //add borders
        if (handDelta.magnitude > joyStickDeadZone)      //don't do anything if not moved much
            telemetryController.cmdPos += timeCorrectedDelta;
        rotateDrone(h);
    }
    void positionControl(Hand h)
    {

        if (!handOpen(h))       //if hand is closed, update offset
        {
            updateDynamicOffset(h);
            return;
        }
        Vector3 handDelta = (h.PalmPosition  - initialHandPos) * positionCtrlScale;
        telemetryController.cmdPos = initialDronePos + handDelta;
        rotateDrone(h);
    }
    void rotateDrone(Hand h)
    {
        /////rotation:
        float handRotation = h.Rotation.eulerAngles.y;
        if (handRotation < 180 && handRotation > rotationThreshPos)
        {
            telemetryController.cmdRot -= Time.deltaTime * rotationSpeed * (handRotation - rotationThreshPos);
        }
        else if (handRotation > 180 && handRotation < rotationThreshNeg)
        {
            telemetryController.cmdRot += Time.deltaTime * rotationSpeed * (rotationThreshNeg - handRotation);
        }
    }
    bool checkForLand()     //land if conditions are met, returns true if time to land
    {
        if (transform.position.y < landHeightTrigger)
        {
            telem.takeoffOUT = 0f;
            return true;
        }
        else
        {
            return false;
        }

    }
    void joystickCtrlHaptics(Hand h)
    {
        if (hapticDevice.GetSensation() != joyCtrlSens)
            hapticDevice.SetSensation(joyCtrlSens);
        if (handOpen(h))
            hapticDevice.StartEmitter();
        else
            hapticDevice.StopEmitter();

        childPlane.transform.localEulerAngles = new Vector3(0, -childPlane.transform.parent.eulerAngles.y, 0);       //rotate sensation opposite of parent
        //lock position 
        joyCtrlSens.lockPos = initialHandPos - h.PalmPosition;
        //open/close:
        Vector3 handDelta = (h.PalmPosition - initialHandPos) * joystickCtrlScale;
        if (handDelta.y > joyStickHeightHapticDeadZone)         //only do circles if really flying up or down
        {
            joyCtrlSens.scanning = true;
            joyCtrlSens.scanDirection = CircleScanDirection.Opening;
        }
        else if (handDelta.y < -joyStickHeightHapticDeadZone)
        {
            joyCtrlSens.scanning = true;
            joyCtrlSens.scanDirection = CircleScanDirection.Closing;
        }
        else
        {
            joyCtrlSens.scanning = false;
        }
    }
    public GameObject childPlane;       //child plane of haptic plane sensation target
    public Vector3 originalScale = new Vector3(0,0,0);
    void positionCtrlHaptics(Hand h)
    {
        if (hapticDevice.GetSensation() != posCtrlSens)
            hapticDevice.SetSensation(posCtrlSens);
        if (handOpen(h))
            hapticDevice.StartEmitter();
        else
            hapticDevice.StopEmitter();

        //modify rotation and scale
        //childPlane.transform.localEulerAngles = new Vector3(0, transform.eulerAngles.y, 0);       //rotate sensation with the drone
        if (originalScale == new Vector3(0, 0, 0))          //initialize
            originalScale = hapticDevice.GetSensation().scale;
        //if (transform.position.y > 0)
        //    hapticDevice.GetSensation().scale = originalScale * transform.position.y;

    }
    bool handOpen(Hand h)
    {
        if (h == null) return false;
        return
        h.GetThumb().IsExtended &&
        h.GetIndex().IsExtended &&
        h.GetMiddle().IsExtended &&
        h.GetRing().IsExtended &&
        h.GetPinky().IsExtended;
    }
    void updateToggles()
    {
        armTog.isOn = telem.armOUT != 0;      //!=0 converts into to bool
        fistTog.isOn = closedHandSeen;
        takeoffTog.isOn = telem.takeoffOUT != 0;
        backBtn.interactable = telem.takeoffState == 0 || telem.state == 9 ;        //before takeoff, after land, and after emergency land
    }
}