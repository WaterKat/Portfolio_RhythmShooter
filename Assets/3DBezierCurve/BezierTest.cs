using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BezierTest : MonoBehaviour
{
    public Bezier bezier;
    [Range(0, 10)]
    public float testInput = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = bezier.CalculateFullBezier(testInput);
    }
}
