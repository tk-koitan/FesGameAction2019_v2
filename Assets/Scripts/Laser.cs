using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public ParticleSystem perLaser;
    public ParticleSystem perThunder;
    private Vector2 startPos;
    private Vector2 targetPos;
    private float xSize = 45f;
    private float xShiftPos = 3.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tmpPos = Input.mousePosition;
        // Z軸修正
        tmpPos.z = 10f;
        // マウス位置座標をスクリーン座標からワールド座標に変換する
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(tmpPos);

        startPos = mousePos;

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.left, 5);

        if (hit)
        {
            targetPos = hit.point;
            transform.position = hit.point;
            Debug.DrawLine(mousePos, hit.point, Color.black);
        }
        else
        {
            targetPos = mousePos + Vector3.left * 5;
            transform.position = mousePos + Vector3.left * 5;
            Debug.DrawRay(mousePos, Vector2.left * 5);
        }
        float distance = (targetPos - (Vector2)mousePos).magnitude;
        Debug.Log(distance);
        Vector3 tmpScale = perLaser.transform.localScale;
        tmpScale.x = distance / xSize;
        perLaser.transform.localScale = tmpScale;
        tmpPos = transform.position;
        tmpPos.z += (1 - tmpScale.x) * xShiftPos;
        perLaser.transform.position = tmpPos;
    }
}
