using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRB : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D box;
    public float maxVx = 5;
    public float accelVx = 1;
    public float gravity = -0.2f;
    public float maxVy = 10;
    public bool isGround;

    private Vector2 v = Vector2.zero;
    private Vector2 rv = Vector2.zero;
    private Vector2 power = Vector2.zero;
    public Vector2 groundPoint;
    private Vector2 groundNormal;
    private GameObject groundObj;
    public float slopeMaxDeg = 50;

    private bool isContactRight;
    private bool isContactLeft;
    private bool isContactUp;


    [SerializeField] ContactFilter2D filter2d;
    [SerializeField] ContactPoint2D[] contacts;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            power.x += accelVx;
            power.x = Mathf.Min(power.x, maxVx);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            power.x -= accelVx;
            power.x = Mathf.Max(power.x, -maxVx);
        }
        else
        {
            power.x *= 0.8f;
        }

        if(isContactRight)
        {
            power.x = Mathf.Min(power.x, 0);
        }

        if(isContactLeft)
        {
            power.x = Mathf.Max(power.x, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            power.y = maxVy;
            isGround = false;
        }

        if (isGround)
        {
            power.y = 0;
            if(power.magnitude<0.01f)
            {
                v = Vector2.zero;
            }
            v = Vector3.ProjectOnPlane(power, groundNormal);
            Debug.DrawRay(groundPoint, groundNormal, Color.red);
            //Debug.DrawRay(groundPoint, v, Color.red);
        }
        else
        {
            power.y += gravity;
            v = power;
        }

        rb.velocity = v + rv;
        //rb.MovePosition(transform.position + (Vector3)v);
        rv = Vector2.zero;
        //transform.position += (Vector3)v;
        Debug.DrawRay(transform.position, v, Color.white);
        //Debug.Log("速さ:"+v.magnitude);
    }

    public void PositionSet(Vector2 velocity)
    {
        //rb.MovePosition(transform.position + (Vector3)velocity);
        transform.position += (Vector3)velocity;
        //v += velocity;
        rv = velocity;
        //rb.velocity += velocity;
        Debug.Log(velocity);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (v.y > 0 && !isGround) return;
        isContactRight = false;
        isContactLeft = false;

        groundPoint = collision.contacts[0].point;
        groundNormal = collision.contacts[0].normal;
        GameObject groundObj = collision.contacts[0].collider.gameObject;
        Vector2 center = transform.position;
        float dir = (v  + center - collision.contacts[0].point).magnitude;

        float slopeMinY = Mathf.Sin((90 - slopeMaxDeg) * Mathf.Deg2Rad);
        for (int i = 1; i < collision.contacts.Length; i++)
        {
            float tmpDir = (v + center - collision.contacts[i].point).magnitude;
            if(tmpDir < dir)
            {
                groundPoint = collision.contacts[i].point;
                groundNormal = collision.contacts[i].normal;
                groundObj = collision.contacts[i].collider.gameObject;
            }
            Debug.DrawRay(collision.contacts[i].point, collision.contacts[i].normal);
        }

        if(isGround)
        {

            RaycastHit2D hit = Physics2D.Raycast(center, Vector2.down * (v.magnitude + box.bounds.size.y));
            Debug.DrawRay(center, Vector2.down * (v.magnitude + box.bounds.size.y));
            if (hit)
            {
                float tmpDir = (v + center - hit.point).magnitude;
                if (tmpDir < dir)
                {
                    groundPoint = hit.point;
                    groundNormal = hit.normal;
                    groundObj = hit.collider.gameObject;
                }
            }
        }


        if (groundNormal.y >= slopeMinY)
        {
            isGround = true;
            if(groundObj.layer == (int)LayerName.MovingPlatform)
            {
                groundObj.GetComponent<Mover>().ridingPlayers.Add(this);
                Debug.Log(groundObj);
            }
            Debug.DrawRay(groundPoint, groundNormal);
            Debug.Log(groundNormal + ">=" + slopeMinY);
        }
        else
        {
            isGround = false;
            if(groundNormal.x<0)
            {
                isContactRight = true;
            }
            else if(groundNormal.x>0)
            {
                isContactLeft = true;
            }
            groundNormal = Vector2.up;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGround = isContactLeft = isContactRight = false;
        groundNormal = Vector2.up;
    }
}
