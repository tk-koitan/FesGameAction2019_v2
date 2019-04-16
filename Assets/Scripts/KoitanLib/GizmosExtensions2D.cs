using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KoitanLib
{
    public static class GizmosExtensions2D
    {
        public static void DrawWireArc2D(Vector3 center, float radius, float startAngle, float endAngle, int segments = 20)
        {
            /*
            var sceneCamera = UnityEditor.SceneView.currentDrawingSceneView.camera;
            var cameraDistance = Vector3.Distance(sceneCamera.transform.position, center);
            Debug.Log(cameraDistance);
            segments = (int)(1000 / cameraDistance);
            segments = Mathf.Clamp(segments, 0, 36);
            UnityEditor.Handles.ArrowCap(0, center, Quaternion.Euler(0,90,0), 1);
            */
            if (radius == 0 || segments == 0) return;
            Vector3 from = center + new Vector3(radius * Mathf.Cos(startAngle * Mathf.Deg2Rad), radius * Mathf.Sin(startAngle * Mathf.Deg2Rad));
            var step = (endAngle - startAngle) / segments;
            if (step == 0) return;
            for (float i = startAngle + step; i <= endAngle; i += step)
            {
                var to = center + new Vector3(radius * Mathf.Cos(i * Mathf.Deg2Rad), radius * Mathf.Sin(i * Mathf.Deg2Rad));
                Gizmos.DrawLine(from, to);
                from = to;
            }
        }

        public static void DrawWireCircle2D(Vector3 center, float radius, int segments = 20)
        {
            DrawWireArc2D(center, radius, 0, 360, segments);
        }

        public static void DrawArrow2D(Vector3 from, Vector3 to, float arrowHeadLength = 1f, float arrowHeadAngle = 20.0f)
        {
            Vector3 direction = (to - from).normalized;
            Gizmos.DrawLine(from, to);
            Gizmos.DrawRay(to, Quaternion.Euler(0, 0, 30) * -direction * arrowHeadLength);
            Gizmos.DrawRay(to, Quaternion.Euler(0, 0, -30) * -direction * arrowHeadLength);
        }

        public static void DrawWireArcArrow2D(Vector3 center, float radius, float startAngle, float endAngle, int segments = 20)
        {
            var step = (endAngle - startAngle) / segments;
            if (step == 0) return;
            DrawWireArc2D(center, radius, startAngle, endAngle - step, segments - 1);
            var from = center + new Vector3(radius * Mathf.Cos((endAngle - step) * Mathf.Deg2Rad), radius * Mathf.Sin((endAngle - step) * Mathf.Deg2Rad));
            var to = center + new Vector3(radius * Mathf.Cos(endAngle * Mathf.Deg2Rad), radius * Mathf.Sin(endAngle * Mathf.Deg2Rad));
            DrawArrow2D(from, to);
        }

        public static void DrawWireRect2D(Vector3 center, float width, float height, float angle = 0)
        {
            Vector3 upperRight = Quaternion.Euler(0, 0, angle) * new Vector3(width / 2, height / 2) + center;
            Vector3 upperLeft = Quaternion.Euler(0, 0, angle) * new Vector3(-width / 2, height / 2) + center;
            Vector3 lowerRight = Quaternion.Euler(0, 0, angle) * new Vector3(width / 2, -height / 2) + center;
            Vector3 lowerLeft = Quaternion.Euler(0, 0, angle) * new Vector3(-width / 2, -height / 2) + center;
            Gizmos.DrawLine(upperRight, upperLeft);
            Gizmos.DrawLine(upperLeft, lowerLeft);
            Gizmos.DrawLine(lowerLeft, lowerRight);
            Gizmos.DrawLine(lowerRight, upperRight);
        }

        public static void DrawBezierCurve2D(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int segments = 20)
        {
            if (segments == 0) return;
            var step = 1f / segments;
            Vector3 from = CubicBezier.GetPoint(p0, p1, p2, p3, 0);
            Vector3 to;
            for (float t = step; t < 1; t += step)
            {
                to = CubicBezier.GetPoint(p0, p1, p2, p3, t);
                Gizmos.DrawLine(from, to);
                from = to;
            }
            to = CubicBezier.GetPoint(p0, p1, p2, p3, 1);
            Gizmos.DrawLine(from, to);
        }

        public static void DrawBezierCurveArrow2D(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int segments = 20)
        {
            if (segments == 0) return;
            var step = 1f / segments;
            Vector3 from = CubicBezier.GetPoint(p0, p1, p2, p3, 0);
            Vector3 to;
            for (float t = step; t < 1; t += step)
            {
                to = CubicBezier.GetPoint(p0, p1, p2, p3, t);
                Gizmos.DrawLine(from, to);
                from = to;
            }
            to = CubicBezier.GetPoint(p0, p1, p2, p3, 1);
            Gizmos.DrawLine(from, to);
            from = CubicBezier.GetPoint(p0, p1, p2, p3, 0.5f - step);
            to = CubicBezier.GetPoint(p0, p1, p2, p3, 0.5f);
            DrawArrow2D(from, to);
        }

        public static Vector3 GetNearestPointBezierCurveArrow2D(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector2 mousePos)
        {
            float distance = float.MaxValue;
            Vector3 point = p0;
            int segments = 20;//とりあえず計算量を考慮して固定
            var step = 1f / segments;
            Vector3 from = p0;
            Vector3 to;
            for (float t = step; t < 1; t += step)
            {
                to = CubicBezier.GetPoint(p0, p1, p2, p3, t);
                Gizmos.DrawLine(from, to);
                Vector3 tmpVec = MidD2(mousePos, from, to);
                if(tmpVec.z < distance)
                {
                    distance = tmpVec.z;
                    point = tmpVec;
                }
                from = to;
            }
            to = p3;
            Gizmos.DrawLine(from, to);
            from = CubicBezier.GetPoint(p0, p1, p2, p3, 0.5f - step);
            to = CubicBezier.GetPoint(p0, p1, p2, p3, 0.5f);
            DrawArrow2D(from, to);
            return point;
        }

        //点と線分の最短距離
        //1,2に座標,3に距離
        public static Vector3 MidD2(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            float x0 = p0.x, y0 = p0.y, x1 = p1.x, y1 = p1.y, x2 = p2.x, y2 = p2.y;
            var a = x2 - x1;
            var b = y2 - y1;
            var a2 = a * a;
            var b2 = b * b;
            var r2 = a2 + b2;
            var tt = -(a * (x1 - x0) + b * (y1 - y0));
            if (tt < 0)
            {
                return new Vector3(p1.x, p1.y, (x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));
            }
            if (tt > r2)
            {
                return new Vector3(p2.x, p2.y, (x2 - x0) * (x2 - x0) + (y2 - y0) * (y2 - y0));
            }
            var f1 = a * (y1 - y0) - b * (x1 - x0);
            return new Vector3(a * tt / r2 + x1,b * tt / r2 + y1, (f1 * f1) / r2);
        }

    }
}

