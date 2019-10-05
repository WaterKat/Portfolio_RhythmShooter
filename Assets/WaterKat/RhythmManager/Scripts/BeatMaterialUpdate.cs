using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMaterialUpdate : MonoBehaviour
{
    [Range(0,1)]
    public float CurrentBeat = 1;

    public AnimationCurve BeatCurve = new AnimationCurve();

    public Material[] beatMaterials = new Material[0];


    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CurrentBeat = Mathf.Repeat(Time.time, .5f)/.5f;
        CurrentBeat = BeatCurve.Evaluate(1-CurrentBeat);
        foreach (Material material in beatMaterials)
        {
            material.SetFloat("_BeatPosition", CurrentBeat);
        }
    }
}
