using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCtrl : MonoBehaviour
{
    public Telemetry telem;
    public float speed;
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        telem.x = telem.xOut;
        telem.y = telem.yOut;
        telem.z = telem.zOut;
        telem.yaw = telem.rOut;
        telem.armState = telem.armOUT;
        telem.takeoffState = telem.takeoffOUT;
        if(telem.armOUT > 0f)
        {
            if (telem.takeoffOUT > 0f)
            {
                telem.state = 5f;
            }
            else
            {
                telem.state = 2f;
            }
        }
        else
        {
            telem.state = 1f;
        }
        #region key control
        /*
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 position = gameObject.transform.position;
            position.z += Time.deltaTime * speed;
            gameObject.transform.position = position;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 position = gameObject.transform.position;
            position.z -= Time.deltaTime * speed;
            gameObject.transform.position = position;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 position = gameObject.transform.position;
            position.x -= Time.deltaTime * speed;
            gameObject.transform.position = position;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 position = gameObject.transform.position;
            position.x += Time.deltaTime * speed;
            gameObject.transform.position = position;
        }
        if (Input.GetKey(KeyCode.E))
        {
            Vector3 rotation = gameObject.transform.eulerAngles;
            rotation.y += Time.deltaTime * rotationSpeed;
            gameObject.transform.eulerAngles = rotation;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Vector3 rotation = gameObject.transform.eulerAngles;
            rotation.y -= Time.deltaTime * rotationSpeed;
            gameObject.transform.eulerAngles = rotation;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 position = gameObject.transform.position;
            position.y += Time.deltaTime * speed;
            gameObject.transform.position = position;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 position = gameObject.transform.position;
            position.y -= Time.deltaTime * speed;
            gameObject.transform.position = position;
        }
        */
        #endregion

    }
}
