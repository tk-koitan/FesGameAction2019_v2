using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour
{
    [System.NonSerialized] public float dir = 0f;

    public float speed = 30f;
    public float actionSpeed = 1.0f;

    private float actionTime = 0.0f;
    public float lifeTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        dir -= 90f;

        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x, transform.localEulerAngles.y, dir + 180f);

        Destroy(gameObject, lifeTime / actionSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = speed * actionSpeed * Time.deltaTime * Mathf.Cos(dir / 180f * Mathf.PI);
        float moveY = speed * actionSpeed * Time.deltaTime * Mathf.Sin(dir / 180f * Mathf.PI);

        Debug.Log(moveX + " " + moveY);
        transform.position += new Vector3(moveX, moveY);
    }
}
