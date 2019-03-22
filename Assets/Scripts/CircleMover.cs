using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using Utils;
using KoitanLib;

public class CircleMover : Mover
{
    public float radius = 5;
    public float duration = 1;
    public Ease ease = Ease.Linear;
    public LoopType loopType = LoopType.Restart;
    private float angle = 0;
    public float startAngle = 0;
    public float maxAngle = 360;
    public float delay = 0;
    public Vector3 pivot;

    // Start is called before the first frame update
    protected override void Start()
    {
        pivot = transform.position;
        angle = startAngle;
        transform.position = pivot + new Vector3(radius * Mathf.Cos(angle * Mathf.Deg2Rad), radius * Mathf.Sin(angle * Mathf.Deg2Rad));
        base.Start();
        DOTween.To(() => angle,t => angle = t,maxAngle,duration).SetRelative().SetEase(ease).SetLoops(-1,loopType).SetDelay(delay);
    }

    protected override void Update()
    {
        transform.position = pivot + new Vector3(radius * Mathf.Cos(angle * Mathf.Deg2Rad), radius * Mathf.Sin(angle * Mathf.Deg2Rad));
        base.Update();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!EditorApplication.isPlaying)
        {
            pivot = transform.position;
        }
        Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(pivot, radius);
        //GizmosExtensions.DrawWireArc(pivot, radius, maxAngle, 36 ,Quaternion.Euler(-startAngle, 90, 90));
        GizmosExtensions2D.DrawWireCircle2D(pivot + new Vector3(radius * Mathf.Cos((startAngle - delay / duration * maxAngle) * Mathf.Deg2Rad), radius * Mathf.Sin((startAngle - delay / duration * maxAngle) * Mathf.Deg2Rad)), radius / 8, 20);
        //GizmosExtensions2D.DrawWireArc2D(pivot, radius, startAngle, startAngle + maxAngle, 36);
        GizmosExtensions2D.DrawWireArcArrow2D(pivot, radius, startAngle, startAngle + maxAngle, 36);
    }
#endif
}
