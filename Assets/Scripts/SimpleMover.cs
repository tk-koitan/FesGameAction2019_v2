using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KoitanLib;
using UnityEditor;

public class SimpleMover : Mover
{
    public float xAmount = 0;
    public float yAmount = 0;
    public float duration = 1;
    public Ease ease = Ease.Linear;
    private int loopTime = 0;
    private Vector2 startPos;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        startPos = currentPos;
        transform.DOMove(new Vector3(xAmount,yAmount),duration).SetRelative().SetEase(ease).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!EditorApplication.isPlaying)
        {
            startPos = transform.position;
        }
        Gizmos.color = Color.white;
        GizmosExtensions2D.DrawArrow2D(startPos, startPos + new Vector2(xAmount, yAmount));
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
