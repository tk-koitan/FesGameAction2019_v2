using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour
{
    [System.NonSerialized] public float dir = 0f;

    private float pompScaleX = 1.0f;
    private float pompScaleY;
    private float basScaleY;
    private float scaleYPerF;
    public float maxScaleY = 1.0f;

    public float speed = 30f;
    public float actionSpeed = 1.0f;

    private float actionTime = 0.0f;
    public float lifeTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        dir -= 90f;

        basScaleY = transform.localScale.y;
        scaleYPerF = (maxScaleY - basScaleY) / 0.5f;

        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x, transform.eulerAngles.y, dir);

        Destroy(gameObject, lifeTime / actionSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = -speed * actionSpeed * Time.deltaTime * Mathf.Cos(dir / 180f * Mathf.PI);
        float moveY = -speed * actionSpeed * Time.deltaTime * Mathf.Sin(dir / 180f * Mathf.PI);

        transform.position += new Vector3(moveX, moveY);

        // y軸の大きさも0.5からmaxScaleYまで増やす これは最初の短時間で
        if (actionTime < 0.5f) pompScaleY = scaleYPerF * actionTime + basScaleY;
        else pompScaleY = maxScaleY;
        transform.localScale = new Vector2(transform.localScale.x, pompScaleY);

        pompScaleX += moveX * 2.0f * 6.0f;

        // 見た目の大きさとあたり判定の大きさを変える x軸
        GetComponent<SpriteRenderer>().size = new Vector2(pompScaleX, 1.5f);
        GetComponent<BoxCollider2D>().size = new Vector2(pompScaleX, 1.5f);

        actionTime += Time.deltaTime * actionSpeed;
    }
}
