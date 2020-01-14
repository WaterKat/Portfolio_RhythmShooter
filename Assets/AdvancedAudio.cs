using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedAudio : MonoBehaviour
{
    int AudioSampleRate = 44100;
    
    public int PrecisePosition(AudioSource audioSource)
    {
        return audioSource.timeSamples;
    }
    public double deltaDoublePosition(AudioSource audioSource)
    {
        return (double)audioSource.timeSamples / (double)audioSource.clip.samples;
    }

    public class BeatMap
    {
        public int[] beatPositions;

        void GenerateBasicBeatMap(int bpm, int sampleLength, int sampleOffset)
        {
            int offsetLength = (sampleLength - sampleOffset);
            double bps = bpm / 60;

        }
    }
}
