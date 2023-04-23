using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ObjectToCanvasRaycaster))]
public class ObjectToCanvasRaycasterCustomEditorDrawer : Editor
{
    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector();

        ObjectToCanvasRaycaster myScript = (ObjectToCanvasRaycaster)target;

        if (GUILayout.Button("Open File"))
        {
            myScript.FitToObject();
        }
    }
}