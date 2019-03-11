using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierPathMover))]
public class BezierPathInspector : Editor
{
    bool snapping;
    int editingIndex;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        snapping = EditorGUILayout.Toggle("snapping", snapping);
    }

    void OnSceneGUI()
    {
        Tools.current = Tool.None;
        var component = target as BezierPathMover;
        var transform = component.transform;
        var path = component.path;
        Vector2 tmpPos = component.transform.position;
        for (int i = 0; i < path.Length; i++)
        {
            tmpPos = path[i].Anchore + (Vector2)component.pivot;
            Handles.color = i == editingIndex ? Color.red : Color.white;

            if(HandleMove(ref tmpPos))
            {
                editingIndex = i;
                Undo.RecordObject(component, "Changed point anchore");
                tmpPos -= (Vector2)component.pivot;
                if (snapping) tmpPos = SnapVec2(tmpPos, 2);
                path[i].Anchore = tmpPos;
                EditorUtility.SetDirty(component);
            }

            tmpPos = path[i].Anchore + path[i].Handle1 + (Vector2)component.pivot;
            if (HandleMove(ref tmpPos))
            {
                editingIndex = i;
                Undo.RecordObject(component, "Changed point left tangent");
                tmpPos -= (Vector2)component.pivot + path[i].Anchore;
                if (snapping) tmpPos = SnapVec2(tmpPos, 2);
                path[i].Handle1 = tmpPos;
                EditorUtility.SetDirty(component);
            }

            tmpPos = path[i].Anchore + path[i].Handle2 + (Vector2)component.pivot;
            if (HandleMove(ref tmpPos))
            {
                editingIndex = i;
                Undo.RecordObject(component, "Changed point right tangent");
                tmpPos -= (Vector2)component.pivot + path[i].Anchore;
                if (snapping) tmpPos = SnapVec2(tmpPos, 2);
                path[i].Handle2 = tmpPos;
                EditorUtility.SetDirty(component);
            }
        }
    }
    

    Vector3 PositionHandle(Transform transform)
    {
        var position = transform.position;

        Handles.color = Handles.xAxisColor;
        position = Handles.Slider(position, transform.right); //X 軸

        Handles.color = Handles.yAxisColor;
        position = Handles.Slider(position, transform.up); //Y 軸

        return position;
    }

    Vector2 SnapVec2(Vector2 vec, int divisor)
    {
        return new Vector2(Mathf.Floor(vec.x * divisor + 0.5f) / divisor, Mathf.Floor(vec.y * divisor + 0.5f) / divisor);
    }

    public bool HandleMove(ref Vector2 position)
    {
        //float size = HandleUtility.GetHandleSize(position) * 0.3f;
        float size = 0.5f;
        EditorGUI.BeginChangeCheck();
        //position = Handles.FreeMoveHandle(position, Quaternion.identity, size, Vector3.zero, Handles.RectangleCap);
        position = Handles.FreeMoveHandle(position, Quaternion.identity, size, Vector3.zero, Handles.DotHandleCap);
        return EditorGUI.EndChangeCheck();
    }
}