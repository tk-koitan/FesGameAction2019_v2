using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBTest : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D box;
    public float maxVx = 5;
    public float accelVx = 1;
    public float gravity = -0.2f;
    public float maxVy = 10;
    public bool isGround;

    private Vector2 v = Vector2.zero;
    private Vector2 groundPoint;
    private Vector2 groundNormal;
    public float slopeMaxDeg = 50;

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
            v.x += accelVx;
            v.x = Mathf.Min(v.x, maxVx);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            v.x -= accelVx;
            v.x = Mathf.Max(v.x, -maxVx);
        }
        else
        {
            v.x *= 0.8f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            v.y = maxVy;
            isGround = false;
        }

        if (isGround)
        {
            v.y = 0;
            v = Vector3.ProjectOnPlane(v, groundNormal);
            Debug.DrawRay(groundPoint, groundNormal, Color.red);
            Debug.DrawRay(groundPoint, v, Color.red);
        }
        else
        {
            v.y += gravity;
        }

        //rb.velocity = v;
        rb.MovePosition(transform.position + (Vector3)v);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (v.y > 0) return;
        groundPoint = collision.contacts[0].point;
        groundNormal = collision.contacts[0].normal;

        float slopeMinY = Mathf.Sin((90 - slopeMaxDeg) * Mathf.Deg2Rad);
        for (int i = 1; i < collision.contacts.Length; i++)
        {
            if(collision.contacts[i].point.y < groundPoint.y)
            {
                groundPoint = collision.contacts[i].point;
                groundNormal = collision.contacts[i].normal;
            }
        }

        if(groundNormal.y >= slopeMinY)
        {
            isGround = true;
            Debug.DrawRay(groundPoint, groundNormal);
            Debug.Log(groundNormal + ">=" + slopeMinY);
        }
        else
        {
            isGround = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGround = false;
    }
}
