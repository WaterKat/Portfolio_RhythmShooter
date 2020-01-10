using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTest : MonoBehaviour
{
    public GameObject display0;
    public GameObject display1;

    public double startTime;
    public double apexTime;

    void Start()
    {
        
    }

    void Update()
    {

        transform.position = Vector3.Lerp(display0.transform.position, display1.transform.position, Mathf.InverseLerp((float)startTime, (float)apexTime, (float)RhythmManager.musicPosition));
    }
}
