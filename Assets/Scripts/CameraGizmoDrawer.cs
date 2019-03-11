using UnityEngine;
using System.Collections;

/*
 * 非選択時にもカメラのビュー範囲を表示する
 */

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraGizmoDrawer : MonoBehaviour
{
    public Color gizmosColor = Color.gray;

    void OnDrawGizmos()
    {
        float fov = Camera.main.fieldOfView;
        float size = Camera.main.orthographicSize;
        float max = Camera.main.farClipPlane;
        float min = Camera.main.nearClipPlane;
        float aspect = Camera.main.aspect;

        Color tempColor = Gizmos.color;
        Gizmos.color = gizmosColor;

        Matrix4x4 tempMat = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(this.transform.position, this.transform.rotation, new Vector3(aspect, 1.0f, 1.0f));

        if (Camera.main.orthographic)
        {
            //OrthoGraphicカメラ設定
            Gizmos.DrawWireCube(new Vector3(0.0f, 0.0f, ((max - min) / 2.0f) + min), new Vector3(size * 2.0f, size * 2.0f, max - min));
        }
        else
        {
            //Perspectiveカメラ設定
            Gizmos.DrawFrustum(Vector3.zero, fov, max, min, 1.0f);
        }

        Gizmos.color = tempColor;
        Gizmos.matrix = tempMat;
    }
}