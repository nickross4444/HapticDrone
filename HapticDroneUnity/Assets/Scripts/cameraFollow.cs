using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public GameObject objectToFollow;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = gameObject.transform.position - objectToFollow.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = objectToFollow.transform.position + offset;
    }
}
