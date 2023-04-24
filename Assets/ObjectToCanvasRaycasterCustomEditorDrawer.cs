using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ObjectToCanvasRaycaster))]
public class ObjectToCanvasRaycasterCustomEditorDrawer : Editor
{
    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector();

        ObjectToCanvasRaycaster ObjectToCanvasRaycasterInstance = (ObjectToCanvasRaycaster)target;

        if (GUILayout.Button("FitCanvasToPage"))
        {
            ObjectToCanvasRaycasterInstance.FitToObject();
        }
        if (GUILayout.Button("TurnPageLeftToRight"))
        {
            ObjectToCanvasRaycasterInstance.FlipPageLeftToRight();
        }
        if (GUILayout.Button("TurnPageRightToLeft"))
        {
            ObjectToCanvasRaycasterInstance.FlipPageRightToLeft();
        }
    }
}