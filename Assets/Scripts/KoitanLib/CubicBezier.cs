using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class CubicBezier
{
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        var oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * oneMinusT * p0 +
            3f * oneMinusT * oneMinusT * t * p1 +
            3f * oneMinusT * t * t * p2 +
            t * t * t * p3;
    }
}

[System.Serializable]
public class ControlPoint
{
    public float duration = 1;
    public Ease ease;
    public Vector2 Anchore;
    public Vector2 Handle1;
    public Vector2 Handle2;
}