using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SensationExplorer : MonoBehaviour
{
    public List<Sensation> sensations = new List<Sensation>();

    private int _sensationIndex;
    private int SensationIndex
    {
        get
        {
            return _sensationIndex;
        }
        set
        {
            _sensationIndex = (value < 0 ? sensations.Count + value : value) % sensations.Count;
        }
    }

    void Start()
    {
        GetComponent<DeviceManager>()?.SetSensation(sensations.First());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SensationIndex++;
            GetComponent<DeviceManager>()?.SetSensation(sensations.ElementAt(SensationIndex));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SensationIndex--;
            GetComponent<DeviceManager>()?.SetSensation(sensations.ElementAt(SensationIndex));
        }
    }
}
