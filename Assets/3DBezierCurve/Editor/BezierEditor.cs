
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bezier))]
public class LookAtPointEditor : Editor
{
    SerializedProperty StartNode;

    SerializedProperty MidNodes;

    SerializedProperty EndNode;

    int EditMode = 0;

    void OnEnable()
    {
        StartNode = serializedObject.FindProperty("StartNode");

        MidNodes = serializedObject.FindProperty("MidNodes");

        EndNode = serializedObject.FindProperty("EndNode");
    }

    protected virtual void OnSceneGUI()
    {
        Bezier BezierCurve = (Bezier)target;

        Event e = Event.current;
        switch (e.type)
        {
            case EventType.KeyDown:
                switch (e.character)
                {
                    case ',':
                        EditMode = 0;
                        break;
                    case '.':
                        EditMode = 1;
                        break;
                    case '/':
                        EditMode = 2;
                        break;
                    case 'n':
                        AddNode(BezierCurve);
                        break;
                    case 'x':
                        RemoveNode(BezierCurve);
                        break;
                }
                break;
        }

        switch (EditMode)
        {
            case 0:
                break;
            case 1:
                EditRootNodes(BezierCurve);
                break;
            case 2:
                EditTangentNodes(BezierCurve);
                break;
        }

    }

    public void AddNode(Bezier BezierCurve)
    {
        GameObject template = BezierCurve.transform.Find("Template").gameObject;
        GameObject newNode = Instantiate(template, BezierCurve.transform);
        newNode.name = "Point" + (BezierCurve.NodeLength - 1);
        newNode.SetActive(true);

        Bezier.BezierNode bezierNode = new Bezier.BezierNode();
        bezierNode.RootNode = newNode.transform;
        bezierNode.BeforeNode = newNode.transform.Find("KnobA");
        bezierNode.AfterNode = newNode.transform.Find("KnobB");

        bezierNode.RootNode.position = BezierCurve.EndNode.RootNode.position;
        bezierNode.BeforeNode.position = BezierCurve.EndNode.BeforeNode.position;
        bezierNode.AfterNode.position = (BezierCurve.EndNode.RootNode.position-BezierCurve.EndNode.BeforeNode.position)+ BezierCurve.EndNode.RootNode.position;

        BezierCurve.EndNode.RootNode.position += Vector3.up*5;

        BezierCurve.MidNodes.Add(bezierNode);
    }

    public void RemoveNode(Bezier BezierCurve)
    {
        if (BezierCurve.MidNodes.Count < 1) { return; }
        BezierCurve.EndNode.RootNode.position = BezierCurve.MidNodes[BezierCurve.MidNodes.Count - 1].RootNode.position;
        BezierCurve.EndNode.BeforeNode.position = BezierCurve.MidNodes[BezierCurve.MidNodes.Count - 1].BeforeNode.position;

        DestroyImmediate(BezierCurve.MidNodes[BezierCurve.MidNodes.Count - 1].RootNode);

        BezierCurve.MidNodes.RemoveAt(BezierCurve.MidNodes.Count - 1);
    }

    public void EditTangentNodes(Bezier BezierCurve)
    {
        Vector3[] MidTangentPositions = new Vector3[BezierCurve.MidNodes.Count*2];

        // 

        EditorGUI.BeginChangeCheck();
        for (int j = 0; j < BezierCurve.MidNodes.Count; j++)
        {
            int i = j * 2;
            MidTangentPositions[i] = Handles.PositionHandle(BezierCurve.MidNodes[j].BeforeNode.position, Quaternion.identity);
        }
        if (EditorGUI.EndChangeCheck()) {
            for (int i = 0; i < BezierCurve.MidNodes.Count; i++)
            {
                Undo.RecordObject(BezierCurve.MidNodes[i].BeforeNode, "Change Mid Tangent Position");
                Undo.RecordObject(BezierCurve.MidNodes[i].AfterNode, "Change Mid Tangent Position");

                BezierCurve.MidNodes[i].BeforeNode.position = MidTangentPositions[i * 2];
                Vector3 tangentVector = (BezierCurve.MidNodes[i].RootNode.position - BezierCurve.MidNodes[i].BeforeNode.position).normalized;
                float AfterNodeMagnitude = (BezierCurve.MidNodes[i].AfterNode.position - BezierCurve.MidNodes[i].RootNode.position).magnitude;
                BezierCurve.MidNodes[i].AfterNode.position = BezierCurve.MidNodes[i].RootNode.position+(tangentVector*AfterNodeMagnitude);
            }
        }

        //

        EditorGUI.BeginChangeCheck();
        for (int j = 0; j < BezierCurve.MidNodes.Count; j++)
        {
            int i = j * 2;
            MidTangentPositions[i + 1] = Handles.PositionHandle(BezierCurve.MidNodes[j].AfterNode.position, Quaternion.identity);
        }
        if (EditorGUI.EndChangeCheck())
        {
            for (int i = 0; i < BezierCurve.MidNodes.Count; i++)
            {
                Undo.RecordObject(BezierCurve.MidNodes[i].BeforeNode, "Change Mid Tangent Position");
                Undo.RecordObject(BezierCurve.MidNodes[i].AfterNode, "Change Mid Tangent Position");

                BezierCurve.MidNodes[i].AfterNode.position = MidTangentPositions[(i * 2) + 1];

                Vector3 tangentVector = (BezierCurve.MidNodes[i].RootNode.position - BezierCurve.MidNodes[i].AfterNode.position).normalized;
                float AfterNodeMagnitude = (BezierCurve.MidNodes[i].BeforeNode.position - BezierCurve.MidNodes[i].RootNode.position).magnitude;
                BezierCurve.MidNodes[i].BeforeNode.position = BezierCurve.MidNodes[i].RootNode.position + (tangentVector * AfterNodeMagnitude);
            }
        }

        //

        EditorGUI.BeginChangeCheck();

        Vector3 StartTangentPosition = Handles.PositionHandle(BezierCurve.StartNode.AfterNode.position, Quaternion.identity);
        Vector3 EndTangentPosition = Handles.PositionHandle(BezierCurve.EndNode.BeforeNode.position, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(BezierCurve.StartNode.AfterNode, "Change Start Tangent Position");
            Undo.RecordObject(BezierCurve.EndNode.BeforeNode, "Change End Tangent Position");

            BezierCurve.StartNode.AfterNode.position = StartTangentPosition;
            BezierCurve.EndNode.BeforeNode.position = EndTangentPosition;
        }

    }


    public void EditRootNodes(Bezier BezierCurve)
    {
        EditorGUI.BeginChangeCheck();

        Vector3 StartNodePosition = Handles.PositionHandle(BezierCurve.StartNode.RootNode.position, Quaternion.identity);
        Vector3 EndNodePosition = Handles.PositionHandle(BezierCurve.EndNode.RootNode.position, Quaternion.identity);

        Vector3[] MidNodePositions = new Vector3[BezierCurve.MidNodes.Count];

        for (int i = 0; i < BezierCurve.MidNodes.Count; i++)
        {
            MidNodePositions[i] = Handles.PositionHandle(BezierCurve.MidNodes[i].RootNode.position, Quaternion.identity);
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(BezierCurve.StartNode.RootNode, "Change StartNode Position");
            Undo.RecordObject(BezierCurve.EndNode.RootNode, "Change EndNode Position");

            BezierCurve.StartNode.RootNode.position = StartNodePosition;
            BezierCurve.EndNode.RootNode.position = EndNodePosition;

            for (int i = 0; i < BezierCurve.MidNodes.Count; i++)
            {
                Undo.RecordObject(BezierCurve.MidNodes[i].RootNode, "Change MidNode Position");
                BezierCurve.MidNodes[i].RootNode.position = MidNodePositions[i];
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();
    }
}
