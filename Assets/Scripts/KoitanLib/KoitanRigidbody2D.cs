using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KoitanLib
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class KoitanRigidbody2D : MonoBehaviour
    {
        /// <summary>
        /// 速度。
        /// </summary>
        public Vector2 velocity;

        /// <summary>
        /// 横方向速度。
        /// </summary>
        public float vx;

        /// <summary>
        /// 縦方向速度。
        /// </summary>
        public float vy;

        /// <summary>
        /// 横方向最大速度。
        /// </summary>
        public float vxMax = 0.4f;

        /// <summary>
        /// 縦方向最大速度。
        /// </summary>
        public float vyMax = 0.7f;

        /// <summary>
        /// 重力の影響の割合。
        /// </summary>
        public float gravityScale = 1.0f;

        /// <summary>
        /// 地面と判定する最大角度。
        /// </summary>
        public float maxSlopeAngle = 50;

        /// <summary>
        /// 右側で衝突しているかどうか。
        /// </summary>
        public bool isRightContact { get; private set; }

        /// <summary>
        /// 左側で衝突しているかどうか。
        /// </summary>
        public bool isLeftContact { get; private set; }

        /// <summary>
        /// 上側で衝突しているかどうか。
        /// </summary>
        public bool isUpContact { get; private set; }

        /// <summary>
        /// 下側で衝突しているかどうか。
        /// </summary>
        public bool isDownContact { get; private set; }

        /// <summary>
        /// 地面に立っているかどうか。
        /// </summary>
        public bool isGround { get; private set; }

        /// <summary>
        /// BoxCollider2D.
        /// </summary>
        public BoxCollider2D hitBox { get; private set; }

        /// <summary>
        /// 接地点。
        /// </summary>
        public Vector2 groundPoint { get; private set; }

        public LayerMask collisionMask;
        public float skinWidth = .015f;
        public int horizontalRayCount = 3;
        public int verticalRayCount = 3;
        public float horizontalRaySpacing { get; private set; }
        public float verticalRaySpacing { get; private set; }
        public RaycastOrigins raycastOrigins;

        private void Start()
        {
            hitBox = GetComponent<BoxCollider2D>();
            CalculateRaySpacing();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                vy = 1;
                isGround = false;
            }

            if(isGround)
            {
                vy = -0.1f;
                isGround = true;
            }
            else
            {
                vy -= 0.1f; 
            }

            if (Input.GetKey(KeyCode.D))
            {
                vx = 0.1f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                vx = -0.1f;
            }
            else
            {
                vx = 0;
            }

            Move(new Vector2(vx, vy));
            // 可視化
            Debug.DrawLine(groundPoint, groundPoint + velocity, Color.white);
        }

        public void Move(Vector2 v)
        {
            CalculateRaySpacing();
            UpdateRaycastOrigins();
            vx = v.x;
            vy = v.y;
            ContactHorizontal();
            ContactVertical();
            velocity = new Vector2(vx, vy);
            transform.position += (Vector3)velocity;
        }

        private void ContactHorizontal()
        {
            int cnt = 0;
            float contactPointX = raycastOrigins.bottomRight.x + skinWidth;
            for (int i = 0;i<horizontalRayCount;i++)
            {
                Vector2 start = raycastOrigins.bottomRight + new Vector2(-verticalRaySpacing * cnt, horizontalRaySpacing * i);
                Vector2 end = start + new Vector2(vx + skinWidth,0);
                RaycastHit2D hit = Physics2D.Linecast(start, end);
                Debug.DrawLine(start, end, Color.white);
                if (hit)
                {
                    while(hit.point == start && cnt < verticalRayCount)
                    {
                        cnt++;
                        start.x -= verticalRaySpacing;
                        hit = Physics2D.Linecast(start, end);
                        Debug.DrawLine(start, end, Color.white);
                    }
                    float slopeMaxY = Mathf.Sin((90 - maxSlopeAngle) * Mathf.Deg2Rad);
                    if (hit.normal.y < slopeMaxY)
                    {
                        vx = hit.point.x - contactPointX;
                        //isGround = true;
                    }
                }
            }

            cnt = 0;
            contactPointX = raycastOrigins.bottomLeft.x - skinWidth;
            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 start = raycastOrigins.bottomLeft + new Vector2(verticalRaySpacing * cnt, horizontalRaySpacing * i);
                Vector2 end = start + new Vector2(vx - skinWidth, 0);
                RaycastHit2D hit = Physics2D.Linecast(start, end);
                Debug.DrawLine(start, end, Color.white);
                if (hit)
                {
                    while (hit.point == start && cnt < verticalRayCount)
                    {
                        cnt++;
                        start.x += verticalRaySpacing;
                        hit = Physics2D.Linecast(start, end);
                        Debug.DrawLine(start, end, Color.white);
                    }
                    float slopeMaxY = Mathf.Sin((90 - maxSlopeAngle) * Mathf.Deg2Rad);
                    if (hit.normal.y <= slopeMaxY)
                    {
                        vx = hit.point.x - contactPointX;
                    }
                }
            }
        }

        private void ContactVertical()
        {
            int cnt = 0;
            float contactPointY = raycastOrigins.bottomLeft.y - skinWidth;
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 start = raycastOrigins.bottomLeft + new Vector2(vx + verticalRaySpacing * i, horizontalRaySpacing * cnt);
                Vector2 end = start + new Vector2(0, vy - skinWidth);
                RaycastHit2D hit = Physics2D.Linecast(start, end);
                Debug.DrawLine(start, end, Color.white);
                if (hit)
                {
                    while (hit.point == start && cnt < horizontalRayCount)
                    {
                        cnt++;
                        start.y += horizontalRaySpacing;
                        hit = Physics2D.Linecast(start, end);
                        Debug.DrawLine(start, end, Color.white);
                    }
                    float slopeMaxY = Mathf.Sin((90 - maxSlopeAngle) * Mathf.Deg2Rad);
                    if (hit.normal.y > slopeMaxY)
                    {
                        vy = hit.point.y - contactPointY;
                        isGround = true;
                    }
                    else
                    {
                        vy *= hit.normal.y;
                    }
                }
            }

            cnt = 0;
            contactPointY = raycastOrigins.topLeft.y + skinWidth;
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 start = raycastOrigins.topLeft + new Vector2(vx + verticalRaySpacing * i, horizontalRaySpacing * cnt);
                Vector2 end = start + new Vector2(0, vy + skinWidth);
                RaycastHit2D hit = Physics2D.Linecast(start, end);
                Debug.DrawLine(start, end, Color.white);
                if (hit)
                {
                    while (hit.point == start && cnt < horizontalRayCount)
                    {
                        cnt++;
                        start.y -= horizontalRaySpacing;
                        hit = Physics2D.Linecast(start, end);
                        Debug.DrawLine(start, end, Color.white);
                    }
                    float slopeMaxY = Mathf.Sin((90 - maxSlopeAngle) * Mathf.Deg2Rad);
                    if (hit.normal.y < -slopeMaxY)
                    {
                        vy = hit.point.y - contactPointY;
                        //isGround = true;
                    }
                    else
                    {
                        vy *= hit.normal.y;
                    }
                }
            }

        }

        public void UpdateRaycastOrigins()
        {
            Bounds bounds = hitBox.bounds;
            bounds.Expand(skinWidth * -2);

            raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
            raycastOrigins.center = bounds.center;
            groundPoint = new Vector2((bounds.min.x + bounds.max.x)/2, bounds.min.y);
        }

        public void CalculateRaySpacing()
        {
            Bounds bounds = hitBox.bounds;
            bounds.Expand(skinWidth * -2);

            horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
            verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }

        public struct RaycastOrigins
        {
            public Vector2 topLeft, topRight;
            public Vector2 bottomLeft, bottomRight;
            public Vector2 center;
        }
    }
}
