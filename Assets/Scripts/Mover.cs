using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    public List<Player> ridingPlayers = new List<Player>(1);
    protected Vector2 v, beforePos, currentPos;
    protected float angleSpeed, beforeAngle, currentAngle;
    public virtual Vector2 Velocity { get; set; }
    protected virtual void Start()
    {
        currentPos = transform.position;
        v = Vector2.zero;
        beforePos = currentPos;
        currentAngle = transform.rotation.eulerAngles.z;
        angleSpeed = 0;
        beforeAngle = currentAngle;
    }
    protected virtual void LateUpdate()
    {
        currentPos = transform.position;
        v = currentPos - beforePos;
        beforePos = currentPos;
        currentAngle = transform.rotation.eulerAngles.z;
        angleSpeed = currentAngle - beforeAngle;
        beforeAngle = currentAngle;
        for (int i = 0; i < ridingPlayers.Count; i++)
        {
            ridingPlayers[i].PositionSet(v);
            Vector2 fromPos = ridingPlayers[i].groundPoint - (Vector2)transform.position;
            Vector2 toPos = Quaternion.Euler(0, 0, angleSpeed) * fromPos;
            Vector2 tmpV = toPos - fromPos;
            Debug.Log(gameObject.name + ":" + tmpV.magnitude);
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
            ridingPlayers[i].PositionSet(tmpV);
            Debug.DrawLine(transform.position, ridingPlayers[i].transform.position, Color.gray);
            if (i == 1) Debug.Log("同時接触");
        }
        ridingPlayers.Clear();
    }
}

public enum LayerName
{
    StaticEnvironment = 8,
    MovingPlatform = 9
}
