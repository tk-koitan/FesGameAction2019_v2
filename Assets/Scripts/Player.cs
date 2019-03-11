using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Vector2 v;
    public float vxMax = 12f;
    public float vyMax = 60;
    public float g = -1.2f;
    public float slopeMaxDeg;
    public int defaultAirJumpTimes = 0;
    private int airJumpTimes = 0;
    public int defaultJumpFrames = 8;
    private int jumpFrames = 0;
    private bool isJumping;
    private bool isGround;
    private bool isRightContact;
    private bool isLeftContact;
    private bool isUpContact;
    private bool isSquat;
    private bool isOtoto;
    private GameObject groundObject;
    private SpriteRenderer sprite;
    private BoxCollider2D hitBox;
    private Transform ridingTr;
    private Transform beforeRidingTr;


    //座標情報
    Vector2 center;
    public Vector2 groundPoint { get; private set; }
    Vector2 middleRightPoint;
    Vector2 middleLeftPoint;
    Vector2 lowerRightPoint;
    Vector2 lowerLeftPoint;
    Vector2 upperRightPoint;
    Vector2 upperLeftPoint;
    Vector2 groundRightPoint;
    Vector2 groundLeftPoint;
    Vector2 topPoint;
    Vector2 topRightPoint;
    Vector2 topLeftPoint;

    // Start is called before the first frame update
    void Start()
    {
        //sprite = GetComponent<SpriteRenderer>();
        hitBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //左右移動
        if (Input.GetKey(KeyCode.RightArrow))
        {
            v.x += vxMax/10;
            if (v.x > vxMax) v.x = vxMax;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            v.x -= vxMax/10;
            if (v.x < -vxMax) v.x = -vxMax;
        }
        else 
        {
            v.x *= 0.80f;
        }

        //上下移動
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGround)
            {
                if (isSquat && groundObject && groundObject.GetComponent<PlatformEffector2D>())
                {
                    PositionSet(new Vector2(0, -hitBox.size.y / 8));
                    v.y = 0;
                    isGround = false;
                }
                else
                {
                    v.y = vyMax;
                    isGround = false;
                    jumpFrames = defaultJumpFrames;
                    isJumping = true;
                }
            }
            else if (airJumpTimes > 0)
            {
                v.y = vyMax;
                airJumpTimes--;
                jumpFrames = defaultJumpFrames;
                isJumping = true;
            }
        }


        if (Input.GetKey(KeyCode.Space) && jumpFrames > 0)
        {
            jumpFrames--;
        }
        else
        {
            isJumping = false;
        }

        if (isGround)
        {
            isJumping = false;
            airJumpTimes = defaultAirJumpTimes;
            //v.y = -hitBox.size.y/4;
            v.y = 0;

            //しゃがみ
            if (Input.GetKey(KeyCode.DownArrow))
            {
                isSquat = true;
            }
            else
            {
                isSquat = false;
            }
        }
        else
        {
            if (!isJumping)
            {
                v.y += g;
            }

        }


        //座標移動
        center = (Vector2)transform.position + hitBox.offset;
        groundPoint = center + new Vector2(0, -hitBox.size.y / 2);
        middleRightPoint = center + new Vector2(hitBox.size.x / 2, 0);
        middleLeftPoint = center + new Vector2(-hitBox.size.x / 2, 0);
        lowerRightPoint = center + new Vector2(hitBox.size.x / 2, -hitBox.size.y / 3);
        lowerLeftPoint = center + new Vector2(-hitBox.size.x / 2, -hitBox.size.y / 3);
        upperRightPoint = center + new Vector2(hitBox.size.x / 2, hitBox.size.y / 3);
        upperLeftPoint = center + new Vector2(-hitBox.size.x / 2, hitBox.size.y / 3);
        groundRightPoint = center + new Vector2(hitBox.size.x / 3, -hitBox.size.y / 2);
        groundLeftPoint = center + new Vector2(-hitBox.size.x / 3, -hitBox.size.y / 2);
        topPoint = center + new Vector2(0, hitBox.size.y / 2);
        topRightPoint = center + new Vector2(hitBox.size.x / 3, hitBox.size.y / 2);
        topLeftPoint = center + new Vector2(-hitBox.size.x / 3, hitBox.size.y / 2);

        /*
        if (v.x>0)
        {
            isRightContact = CheckWall(downRightPoint, 1) || CheckWall(middleRightPoint, 1);
            isLeftContact = false;
        }
        else if(v.x<0)
        {
            isLeftContact = CheckWall(downLeftPoint, -1) || CheckWall(middleLeftPoint, -1);
            isRightContact = false;
        }
        else   
        */
        {
            isRightContact = CheckWall(lowerRightPoint, 1) || CheckWall(middleRightPoint, 1) || CheckWall(upperRightPoint, 1);
            isLeftContact = CheckWall(lowerLeftPoint, -1) || CheckWall(middleLeftPoint, -1) || CheckWall(upperLeftPoint, 1);
        }

        isGround = CheckGround(groundPoint);
        if(!isGround)
        {
            isGround = CheckGround(groundRightPoint) || CheckGround(groundLeftPoint);
            isOtoto = isGround;
        }
        else
        {
            isOtoto = false;
        }
        isUpContact = CheckTop(topPoint) || CheckTop(topRightPoint) || CheckTop(topLeftPoint);

        //床、この方法ではだめだった
        /*
        if(!isGround)
        {
            ridingTr = null;
        }
        if (!beforeRidingTr && ridingTr)
        {
            transform.SetParent(ridingTr);
        }
        if(beforeRidingTr && !ridingTr)
        {
            transform.SetParent(null);
        }
        if(beforeRidingTr != ridingTr)
        {
            transform.SetParent(ridingTr);
        }
        beforeRidingTr = ridingTr;
        */      

        // 可視化
        Debug.DrawLine(groundPoint, groundPoint + v, Color.white);
        //Debug.DrawLine(middleRightPoint, middleRightPoint + v, Color.white);
        //Debug.DrawLine(middleLeftPoint, middleLeftPoint + v, Color.white);

        //transform.Translate(v);
        PositionSet(v);
        //cameraTr.position = transform.position +  new Vector3(0,0,-10);
    }

    private bool CheckWall(Vector2 pos, float flip)
    {
        RaycastHit2D hit = Physics2D.Linecast(new Vector2(center.x, pos.y), pos + new Vector2(v.x + 0.001f * flip, 0));
        Debug.DrawLine(new Vector2(center.x, pos.y), pos + new Vector2(v.x + 0.001f * flip, 0), Color.yellow);
        if (hit)
        {
            /*
            if (hit.point == pos)
            {
                hit = Physics2D.Linecast(pos + new Vector2(-hitBox.size.x / 4 * flip, 0), pos + new Vector2(v.x, 0));
                Debug.DrawLine(pos + new Vector2(-hitBox.size.x / 4 * flip, 0), pos + new Vector2(v.x, 0), Color.red);
                Debug.DrawRay(hit.point, hit.normal, Color.yellow);
            }
            */
            Debug.DrawRay(hit.point, hit.normal, Color.yellow);
            GameObject tmpObj = hit.collider.gameObject;
            if (tmpObj.GetComponent<PlatformEffector2D>()) return false;
            float slopeMaxY = Mathf.Sin((90 - slopeMaxDeg) * Mathf.Deg2Rad);
            if (hit.normal.y < slopeMaxY)
            {
                v.x = hit.point.x - pos.x;
                return true;
            }
        }
        return false;
    }

    private bool CheckGround(Vector2 pos)
    {
        /*
        if (isRightContact && isLeftContact)
        {
            v.x = 0;
            if(v.y>0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        */
        float groundShiftY = 0;
        if (isGround)
        {
            groundShiftY = vxMax * 1.5f;
        }
        pos.x += v.x;
        RaycastHit2D hit = Physics2D.Linecast(new Vector2(pos.x, center.y), pos + new Vector2(0, v.y - groundShiftY));
        Debug.DrawLine(new Vector2(pos.x, center.y), pos + new Vector2(0, v.y - groundShiftY), Color.yellow);
        if (hit && v.y <= 0)
        {

            if (hit.point == new Vector2(pos.x, center.y))
            {
                hit = Physics2D.Linecast(new Vector2(pos.x, center.y + hitBox.size.y / 2), pos + new Vector2(0, v.y - groundShiftY));
                //v.y = hit.point.y - pos.y;
                //return true;
            }
            Debug.DrawRay(hit.point, hit.normal, Color.red);
            //Debug.Log(Mathf.Atan(hit.normal.y/hit.normal.x)*Mathf.Rad2Deg);
            //Debug.Log(Mathf.Sqrt(Mathf.Pow(hit.normal.x, 2) + Mathf.Pow(hit.normal.y, 2)));
            //Debug.Log(hit.normal.x.ToString() + " ," + hit.normal.y.ToString());
            float slopeMaxY = Mathf.Sin((90 - slopeMaxDeg) * Mathf.Deg2Rad);
            //Debug.Log(slopeMaxY);
            if (hit.normal.y < slopeMaxY)
            {
                v.y *= Mathf.Abs(hit.normal.x);
                return false;
            }
            GameObject tmpObj = hit.collider.gameObject;
            float tmpV = hit.point.y - pos.y;
            if (tmpObj.GetComponent<PlatformEffector2D>() && !isGround && tmpV > hitBox.size.y / 12)
            {
                return false;
            }

            if (tmpObj.layer == (int)LayerName.MovingPlatform)
            {
                ridingTr = tmpObj.transform;
                //v += tmpObj.GetComponent<Mover>().Velocity;
                tmpObj.GetComponent<Mover>().ridingPlayers.Add(this);
                //Debug.Log(tmpObj.GetComponent<PlatformEffector2D>().ToString());
            }
            else
            {
                ridingTr = null;
            }
            v.y = tmpV;
            groundObject = tmpObj;
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckTop(Vector2 pos)
    {
        pos.x += v.x;
        if (v.y >= 0)
        {
            RaycastHit2D hit = Physics2D.Linecast(new Vector2(pos.x, center.y), pos + new Vector2(0, v.y));
            Debug.DrawLine(new Vector2(pos.x, center.y), pos + new Vector2(0, v.y), Color.yellow);
            if (hit)
            {
                GameObject tmpObj = hit.collider.gameObject;
                if (!tmpObj.GetComponent<PlatformEffector2D>())
                {
                    if (isGround && isRightContact && isLeftContact)
                    {
                        return false;
                    }
                    v.y = hit.point.y - pos.y;
                }
                return true;
            }
        }
        return false;
    }


    private void CheckIK(Transform ik, Transform foot, Vector2 pos)
    {
        RaycastHit2D hit = Physics2D.Linecast(new Vector2(pos.x, center.y), new Vector2(pos.x,pos.y));
        Debug.DrawLine(new Vector2(pos.x, center.y), pos + new Vector2(0, -vyMax),Color.black);
        if (hit)
        {
            //ik.position = hit.point;
            ik.position += ((Vector3)hit.point - ik.position) / 4f;
            float deg = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg - 90;
            //foot.rotation = Quaternion.Euler(0, 0, deg);
            //ik.rotation = Quaternion.Euler(0, 0, deg);
            //Debug.Log(deg);
        }
        else
        {
            //ik.position = pos + new Vector2(0, -vyMax);
            ik.position += (Vector3)(pos + new Vector2(0, -vyMax) - (Vector2)ik.position) / 4f;
            //ik.DOMove(pos + new Vector2(0, -vyMax), 0.5f);
            //foot.rotation = Quaternion.Euler(0, 0, 0);
            //ik.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void PositionSet(Vector2 velocity)
    {
        transform.position += (Vector3)velocity;
    }
}
