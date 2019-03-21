using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KoitanLib;
using UnityEditor;

public class BezierMover : Mover
{
    public float speedX = 0;
    public float speedY = 0;
    public float duration = 1;
    public Ease ease = Ease.Linear;
    public LoopType loopType = LoopType.Yoyo;
    private Vector3 startPos;
    private float t = 0;

    [SerializeField]
    private ControlPoint startPoint,endPoint;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        startPos = transform.position;
        DOTween.To(() => t, f => t = f, 1, duration).SetRelative().SetEase(ease).SetLoops(-1, loopType);
        beforePos = transform.position;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        transform.position = CubicBezier.GetPoint(startPoint.Anchore, startPoint.Anchore + startPoint.Handle2, startPoint.Anchore + endPoint.Anchore + endPoint.Handle1, startPoint.Anchore + endPoint.Anchore, t);
        base.FixedUpdate();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying)
        {
            startPos = transform.position;
            startPoint.Anchore = startPos;
        }
        Gizmos.color = Color.white;
        Gizmos.DrawLine(startPoint.Anchore, startPoint.Anchore + startPoint.Handle2);
        Gizmos.DrawLine(startPoint.Anchore + endPoint.Anchore, startPoint.Anchore + endPoint.Anchore + endPoint.Handle1);
        GizmosExtensions2D.DrawBezierCurve2D(startPoint.Anchore, startPoint.Anchore + startPoint.Handle2, startPoint.Anchore + endPoint.Anchore + endPoint.Handle1, startPoint.Anchore + endPoint.Anchore);
        Gizmos.color = Color.red;
        GizmosExtensions2D.DrawWireRect2D(startPoint.Anchore + startPoint.Handle2, 1, 1);
        GizmosExtensions2D.DrawWireRect2D(startPoint.Anchore + endPoint.Anchore + endPoint.Handle1, 1, 1);
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
