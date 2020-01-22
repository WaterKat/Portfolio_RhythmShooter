using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessedBezier : MonoBehaviour
{
    public Bezier DesiredBezier;
    float DistanceBetweenPoints = 1;

    public List<float> LerpID;
    public List<Vector3> PointLists;

    [ContextMenu("ProcessBezier")]
    void ProcessBezier()
    {
        LerpID.Clear();
        PointLists.Clear();

        LerpID.Add(0f);
        PointLists.Add(DesiredBezier.CalculateFullBezier(0f));

        float CurrentLerpID = 0f;
    }
}
