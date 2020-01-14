using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bezier : MonoBehaviour
{
    [System.Serializable]
    public class BezierNode
    {
        public Transform BeforeNode;

        public Transform RootNode;

        public Transform AfterNode;
    }

    public BezierNode StartNode = new BezierNode();

    public List<BezierNode> MidNodes = new List<BezierNode>();

    public BezierNode EndNode = new BezierNode();

    public int NodeLength
    {
        get
        {
            return MidNodes.Count + 2;
        }
    }

    public BezierNode GetClosestNode(int input)
    {
        if (input == 0)
        {
            return StartNode;
        }
        if (input == NodeLength - 1)
        {
            return EndNode;
        }
        else
        {
            return MidNodes[input - 1];
        }
    }

    public Vector3 CalculateFullBezier(float input)
    {
        input = Mathf.Clamp(input, 0, NodeLength-1);

        if (input == 0)
        {
            return StartNode.RootNode.position;
        }
        if (input == NodeLength-1)
        {
            return EndNode.RootNode.position;
        }

        int firstNode = Mathf.FloorToInt(input);

        BezierNode NodeA = GetClosestNode(firstNode);
        BezierNode NodeB = GetClosestNode(firstNode + 1);

        return Vector3Bezier(input%1, NodeA.RootNode.position, NodeA.AfterNode.position, NodeB.BeforeNode.position, NodeB.RootNode.position);
    }

    public Vector3 Vector3Bezier(float input, Vector3 vector0, Vector3 vector1, Vector3 vector2, Vector3 vector3)
    {
        float vector_x = CubicBezier(input, vector0.x, vector1.x, vector2.x, vector3.x);
        float vector_y = CubicBezier(input, vector0.y, vector1.y, vector2.y, vector3.y);
        float vector_z = CubicBezier(input, vector0.z, vector1.z, vector2.z, vector3.z);
        return new Vector3(vector_x, vector_y, vector_z);
    }

    /*
    public float CubicBezier(float input, float Input0, float Input1, float Input2, float Input3)
    {
        float partA = Mathf.Lerp(Input0, Input1, input);
        float partB = Mathf.Lerp(Input1, Input2, input);
        float partC = Mathf.Lerp(Input2, Input3, input);

        float partD = Mathf.Lerp(partA, partB, input);
        float partE = Mathf.Lerp(partB, partC, input);

        float partF = Mathf.Lerp(partD, partE, input);

        return partF;
    }*/

    
    public float CubicBezier(float input, float Input0, float Input1, float Input2,float Input3)
    {
        float partA = Mathf.Pow(1 - input, 3) * Input0;
        float partB = 3 * Mathf.Pow(1 - input, 2) * Input1 * input;
        float partC = 3 * (1 - input) * Mathf.Pow(input, 2) * Input2 ;
        float partD = Mathf.Pow(input, 3) * Input3;
        return partA + partB + partC + partD;
    }
    

    private void OnDrawGizmos()
    {
        for (int i = 0; i < NodeLength-1; i++)
        {
            BezierNode NodeA = GetClosestNode(i);
            BezierNode NodeB = GetClosestNode(i+1);

            Handles.DrawBezier(NodeA.RootNode.position, NodeB.RootNode.position, NodeA.AfterNode.position, NodeB.BeforeNode.position, Color.red, null, 2f);
        }
        
        if (UnityEditor.Selection.activeGameObject == gameObject) return;

        Handles.Label(StartNode.RootNode.transform.position, "0");
        Handles.Label(EndNode.RootNode.transform.position, (MidNodes.Count + 1).ToString());
        for (int i = 0; i < MidNodes.Count; i++)
        {
            Handles.Label(MidNodes[i].RootNode.transform.position, (i + 1).ToString());
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(StartNode.RootNode.transform.position, 0.25f);
        Handles.Label(StartNode.RootNode.transform.position+Vector3.up, "0");

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(StartNode.AfterNode.transform.position, 0.1f);
        Handles.Label(StartNode.AfterNode.transform.position + Vector3.up, "0_");


        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(EndNode.BeforeNode.transform.position, 0.1f);
        Handles.Label(EndNode.BeforeNode.transform.position + Vector3.up, "_"+(MidNodes.Count+1));


        Gizmos.color = Color.black;
        Gizmos.DrawSphere(EndNode.RootNode.transform.position, 0.25f);
        Handles.Label(EndNode.RootNode.transform.position + Vector3.up, (MidNodes.Count + 1).ToString());


        for (int i = 0; i < MidNodes.Count; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(MidNodes[i].BeforeNode.transform.position, 0.1f);
            Handles.Label(MidNodes[i].BeforeNode.transform.position + Vector3.up, "_" + (i + 1));


            Gizmos.color = Color.Lerp(Color.white, Color.black, (float)(i + 1)/(MidNodes.Count+1) );
            Gizmos.DrawSphere(MidNodes[i].RootNode.transform.position, 0.25f);
            Handles.Label(MidNodes[i].RootNode.transform.position + Vector3.up,  (i + 1).ToString() );


            Gizmos.color = Color.red;
            Gizmos.DrawSphere(MidNodes[i].AfterNode.transform.position, 0.1f);
            Handles.Label(MidNodes[i].AfterNode.transform.position + Vector3.up, (i + 1) + "_");
        }
    }
}
