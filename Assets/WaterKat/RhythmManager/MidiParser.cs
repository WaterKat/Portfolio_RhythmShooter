#if UNITY_EDITOR

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SmfLite;

public class MidiParser : MonoBehaviour
{
    [SerializeField]
    public string MidiPath = "";
    [SerializeField]
    MidiFileContainer display;

    [ContextMenu("ConvertMidi")]
    void ConvertMidi()
    {
        Debug.Log("tried o well");
        MidiFileContainer midiFile = MidiFileLoader.Load(File.ReadAllBytes(MidiPath));
        display = midiFile;
        Debug.Log(display.tracks[0].MidiEventPairs[0].midiEvent.data1);
    }
}

#endif