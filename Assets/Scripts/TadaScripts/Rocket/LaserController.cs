using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float defaultScale = 1.0f;
    public float speed = 1.0f;
    public float initSpeed = 3.0f;
    public float dir = 1.0f;

    private float lifeTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        speed *= initSpeed;

        transform.localScale = new Vector3(defaultScale, defaultScale, 1.0f);

        transform.localEulerAngles = new Vector3(0f, 0f, dir);

        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }
}
