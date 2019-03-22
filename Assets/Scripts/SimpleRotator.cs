using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KoitanLib;
using UnityEditor;
using System.Linq;

public class SimpleRotator : Mover
{
    public float maxAngle = 360;
    private float startAngle = 0;
    public float duration = 1;
    public Ease ease = Ease.Linear;
    public LoopType loopType = LoopType.Incremental;
    public float radius = 1;
    private Collider2D collider;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        startAngle = currentAngle;
        transform.DORotate(new Vector3(0,0,maxAngle), duration).SetRelative().SetEase(ease).SetLoops(-1,loopType);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!EditorApplication.isPlaying)
        {
            startAngle = transform.rotation.eulerAngles.z;
            if(!collider)
            {
                collider = GetComponent<Collider2D>();
            }
            if(collider)
            {
                radius = new float[2] {collider.bounds.size.x,collider.bounds.size.y }.Max()/2;
            }
        }
        Gizmos.color = Color.white;
        GizmosExtensions2D.DrawWireArcArrow2D(transform.position, radius, startAngle, startAngle + maxAngle);
        //GizmosExtensions2D.DrawWireRect2D(transform.position, 1, 2, 30);
    }
#endif
}
