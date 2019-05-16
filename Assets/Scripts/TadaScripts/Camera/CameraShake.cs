using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool isShake = false;

    public Vector2 shakePower = new Vector2(1f, 1f);

    private Vector3 defaultPos;

    private void Start()
    {
        defaultPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShake) DOShake();
    }

    private void DOShake()
    {
        float x = defaultPos.x + Random.Range(-shakePower.x, shakePower.x);
        float y = defaultPos.y + Random.Range(-shakePower.y, shakePower.y);

        transform.localPosition = new Vector3(x, y, defaultPos.z);
    }
}
