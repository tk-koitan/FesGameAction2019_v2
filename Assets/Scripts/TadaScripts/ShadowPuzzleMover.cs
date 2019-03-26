using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;

public class ShadowPuzzleMover : IsActionEnabled
{
    public float moveAngle = 0f;
    public float moveDistanceMax = 5.0f;
    public float moveSpeed = 0.05f;
    private float moveDistance = 0.0f;

    protected override void UpdatedAction()
    {
        if (Mathf.Abs(moveDistance + moveSpeed) > moveDistanceMax)
            moveSpeed = (moveSpeed > 0) ? moveDistanceMax - moveDistance : -(moveDistanceMax + moveDistance);

        moveDistance += moveSpeed;
        transform.position += new Vector3(moveSpeed * Mathf.Cos(moveAngle), moveSpeed * Mathf.Sin(moveAngle));

        transform.position += new Vector3(0, 0, 0);
    }

    private void OnDrawGizmos()
    {
        GizmosExtensions2D.DrawArrow2D(transform.position,
        transform.position + new Vector3((moveDistanceMax - moveDistance) * Mathf.Cos(moveAngle) * Mathf.Sign(moveSpeed),
            (moveDistanceMax - moveDistance) * Mathf.Sin(moveAngle)));
    }
}
