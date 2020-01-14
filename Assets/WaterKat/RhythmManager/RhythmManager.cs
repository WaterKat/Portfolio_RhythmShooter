using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RhythmManager : MonoBehaviour
{
    public AudioSource musicSource;
    
    public static double musicPosition;

    public int BPM = 108;
    public int Subdivisions = 4;

    public double SongOffset = 0; //In Seconds

 //   []

    void Update()
    {
        musicPosition = musicSource.timeSamples;
    }
}
