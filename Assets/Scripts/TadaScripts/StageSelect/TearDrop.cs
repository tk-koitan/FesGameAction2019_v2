using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearDrop : MonoBehaviour
{
    public float startSpeedVy = 2.0f;
    public float speedVxMax = 0.1f;

    private float speedVx;
    private float speedVy;
    private float gravity = -0.2f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 4.0f);
        speedVx = Random.Range(-speedVxMax, speedVxMax);
        speedVy = startSpeedVy;
    }

    private void Update()
    {
        speedVy -= gravity;
        transform.position += new Vector3(speedVx, speedVy);
    }
}
