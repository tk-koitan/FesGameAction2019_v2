using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KoitanLib;
using UnityEditor;

public class BezierPathMover : Mover
{
    private int loopTime = 0;
    public Vector3 pivot;
    public LoopType loopType = LoopType.Restart;
    public bool openEnded;
    private int index = 0, maxIndex;
    private float t = 0;
    private float overTime = 0;
    private Vector3 from, fromHandle, toHandle, to;

    public ControlPoint[] path = new ControlPoint[0];

    // Start is called before the first frame update
    protected override void Start()
    {
        pivot = transform.position;
        maxIndex = openEnded ? path.Length : path.Length - 1;
        //バグるので使わない
        if (loopType == LoopType.Incremental) loopType = LoopType.Restart;
        Sequence seq = DOTween.Sequence();

        seq.PrependCallback(() =>
        {
            t = 0;
            index = 0;
            from = pivot + (Vector3)path[index].Anchore;
            fromHandle = from + (Vector3)path[index].Handle2;
            to = pivot + (Vector3)path[index + 1].Anchore;
            toHandle = to + (Vector3)path[index + 1].Handle1;
        }
        );
        for (int i = 0; i < maxIndex; i++)
        {
            seq.Append(DOTween.To(() => overTime, f => overTime = f, 1, path[i].duration).SetEase(path[i].ease)).SetRelative();
            /*
            seq.AppendCallback(() =>
            {
                //index++;
                from = to;
                to = pivot + (Vector3)path[index+1].Anchore;
            });
            */
        }
        if (loopType == LoopType.Incremental)
        {
            seq.OnStepComplete(() =>
            {
                loopTime++;
                pivot = transform.position;
            }
            );
        }
        seq.SetLoops(-1, loopType);

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        t = overTime % 1;
        index = (int)overTime - loopTime * (maxIndex);
        if (index == maxIndex)
        {
            t = 1;
            index = maxIndex - 1;
        }
        from = pivot + (Vector3)path[index].Anchore;
        fromHandle = from + (Vector3)path[index].Handle2;
        int tmpIndex = index + 1;
        if (openEnded && tmpIndex == maxIndex) tmpIndex = 0;
        to = pivot + (Vector3)path[tmpIndex].Anchore;
        toHandle = to + (Vector3)path[tmpIndex].Handle1;
        transform.position = CubicBezier.GetPoint(from, fromHandle, toHandle, to, t);
        base.Update();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying)
        {
            pivot = transform.position;
        }
        //GizmosExtensions2D.DrawArrow2D(startPos, startPos + new Vector3(speedX, speedY));
        if (path.Length > 0)
        {
            Vector3 from = pivot + (Vector3)path[0].Anchore;
            Vector3 to;
            Gizmos.color = Color.white;
            for (int i = 1; i < path.Length; i++)
            {
                to = pivot + (Vector3)path[i].Anchore;
                //GizmosExtensions2D.DrawBezierCurve2D(from, from + (Vector3)path[i - 1].Handle2, to + (Vector3)path[i].Handle1, to);
                GizmosExtensions2D.DrawBezierCurveArrow2D(from, from + (Vector3)path[i - 1].Handle2, to + (Vector3)path[i].Handle1, to);
                from = to;
            }
            if(openEnded)
            {
                to = pivot + (Vector3)path[0].Anchore;
                GizmosExtensions2D.DrawBezierCurveArrow2D(from, from + (Vector3)path[path.Length-1].Handle2, to + (Vector3)path[0].Handle1, to);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!EditorApplication.isPlaying)
        {
            pivot = transform.position;
        }
        if (path.Length > 0)
        {
            Vector3 from = pivot + (Vector3)path[0].Anchore;
            Vector3 to;
            for (int i = 1; i < path.Length; i++)
            {
                Gizmos.color = Color.white;
                to = pivot + (Vector3)path[i].Anchore;
                Gizmos.DrawLine(from, from + (Vector3)path[i - 1].Handle2);
                Gizmos.DrawLine(to, to + (Vector3)path[i].Handle1);
                //Gizmos.color = Color.red;
                //GizmosExtensions2D.DrawWireRect2D(from + (Vector3)path[i - 1].Handle2, 1, 1);
                //GizmosExtensions2D.DrawWireRect2D(to + (Vector3)path[i].Handle1, 1, 1);
                from = to;
            }
            if (openEnded)
            {
                to = pivot + (Vector3)path[0].Anchore;
                Gizmos.DrawLine(from, from + (Vector3)path[path.Length-1].Handle2);
                Gizmos.DrawLine(to, to + (Vector3)path[0].Handle1);
            }
        }
    }
#endif

    public override Vector2 Velocity
    {
        get
        {
            return v;
        }
    }
}

