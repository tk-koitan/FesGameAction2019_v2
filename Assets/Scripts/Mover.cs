using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;

public abstract class Mover : MonoBehaviour
{
    // === tada ============================
    public bool actionEnabled = false;

    // isTriggerがtrueだと、一度でもactionEnabledがtrueになったらずっと実行する
    [SerializeField] private bool isTrigger = false;
    private bool reallyEnabled = false;
    // =====================================

    public List<PlayerRB> ridingPlayers = new List<PlayerRB>(1);
    public Vector2 v, beforePos, currentPos;
    protected float beforeAngle, currentAngle;
    public float angleSpeed { private set; get; }
    public virtual Vector2 Velocity { private set; get; }
    protected virtual void Start()
    {
        currentPos = transform.position;
        v = Vector2.zero;
        beforePos = currentPos;
        currentAngle = transform.rotation.eulerAngles.z;
        angleSpeed = 0;
        beforeAngle = currentAngle;
    }

    protected virtual void Update()
    {
        if (GetIFActionEnabled()) UpdatedAction();


        currentPos = transform.position;
        v = currentPos - beforePos;
        beforePos = currentPos;
        currentAngle = transform.rotation.eulerAngles.z;
        angleSpeed = currentAngle - beforeAngle;
        beforeAngle = currentAngle;
           
        for (int i = 0; i < ridingPlayers.Count; i++)
        {
            ridingPlayers[i].PositionSet(v);
            Vector2 fromPos = ridingPlayers[i].groundPoint - ((Vector2)transform.position);
            Vector2 toPos = Quaternion.Euler(0, 0, angleSpeed) * fromPos;
            Vector2 tmpV = toPos - fromPos;
            //Debug.Log(gameObject.name + ":" + tmpV.magnitude);
            /*
            if (tmpV.magnitude >= 1f)
            {
                ridingPlayers[i].v = tmpV * 4;
                ridingPlayers[i].isGround = false;
            }
            else
            {
                ridingPlayers[i].PositionSet(tmpV);
            }
            */
            Debug.DrawLine(ridingPlayers[i].groundPoint, ridingPlayers[i].groundPoint + tmpV, Color.yellow);
            ridingPlayers[i].PositionSet(tmpV);
            //Debug.DrawLine(transform.position, ridingPlayers[i].transform.position, Color.gray);
            if (i == 1) Debug.Log("同時接触");
        }
        //ridingPlayers.Clear();
    }

    protected virtual void UpdatedAction()
    {
        // 派生クラスで実装
        // この処理は画面外で起こしたくない
        // 取りあえず、プレイヤーもこのギミックの始動要員として画面内のみで可能な処理にしてる
    }

    private bool GetIFActionEnabled()
    {
        if (!actionEnabled && !isTrigger) return false;

        if (!reallyEnabled || !isTrigger)
            reallyEnabled = actionEnabled;

        return reallyEnabled;
    }
}

public enum LayerName
{
    StaticEnvironment = 8,
    MovingPlatform = 9
}
