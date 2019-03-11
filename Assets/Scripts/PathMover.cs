using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KoitanLib;
using UnityEditor;

public class PathMover : Mover
{
    private Vector2 beforePos;
    private int loopTime = 0;
    private Vector3 startPos;
    public LoopType loopType = LoopType.Restart;

    [System.Serializable]
    class Path
    {
        public Vector2 pos;
        public float duration = 1;
        public Ease ease = Ease.Linear;
    }

    [SerializeField]
    Path[] paths = new Path[0];

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        Sequence seq = DOTween.Sequence();
        for(int i = 0;i<paths.Length;i++)
        {
            seq.Append(transform.DOMove(paths[i].pos, paths[i].duration).SetRelative().SetEase(paths[i].ease));
        }
        seq.SetLoops(-1, loopType);
        //transform.DOMove(new Vector3(speedX, speedY), duration).SetRelative().SetEase(ease).SetLoops(-1, LoopType.Yoyo);
        beforePos = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        /*
        loopTime++;
        loopTime = loopTime % (int)(60 * duration);
        if(loopTime< 30 * duration)
        {
            transform.position += new Vector3(speedX / 60, speedY / 60);
        }
        else
        {
            transform.position += new Vector3(-speedX / 60, -speedY / 60);
        }
        */
        v = (Vector2)transform.position - beforePos;
        beforePos = transform.position;
        for (int i = 0; i < ridingPlayers.Count; i++)
        {
            ridingPlayers[i].PositionSet(v);
            Debug.DrawLine(transform.position, ridingPlayers[i].transform.position, Color.gray);
            if (i == 1) Debug.Log("同時接触");
        }
        ridingPlayers.Clear();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying)
        {
            startPos = transform.position;
        }
        //GizmosExtensions2D.DrawArrow2D(startPos, startPos + new Vector3(speedX, speedY));
        if(paths.Length>0)
        {
            Vector3 from = startPos;
            Vector3 to;
            Gizmos.color = Color.white;
            for (int i = 0; i < paths.Length; i++)
            {
                to = from + (Vector3)paths[i].pos;
                GizmosExtensions2D.DrawArrow2D(from, to);
                from = to;
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

