using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private bool isShake = false;

    public Vector2 shakePower = new Vector2(1f, 1f);

    private Vector3 defaultPos;

    private void Start()
    {
        defaultPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShake) Shake();
    }

    private void Shake()
    {
        float x = defaultPos.x + Random.Range(-shakePower.x, shakePower.x);
        float y = defaultPos.y + Random.Range(-shakePower.y, shakePower.y);

        transform.localPosition = new Vector3(x, y, defaultPos.z);
    }

    private void BeginShake()
    {
        isShake = true;
        defaultPos = transform.position;
    }

    private void EndShake()
    {
        isShake = false;
        transform.position = defaultPos;
    }

    public void DOShake()
    {
        BeginShake();
    }

    public void DOShake(float duration)
    {
        StartCoroutine(CameraShakeCoroutine(duration));
    }

    private IEnumerator CameraShakeCoroutine(float time)
    {
        BeginShake();
        yield return new WaitForSeconds(time);
        EndShake();
    }
}
