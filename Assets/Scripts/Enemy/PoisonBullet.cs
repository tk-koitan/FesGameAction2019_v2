using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBullet : Bullet
{
    public Vector3 rotationSpeed;
    public GameObject DestroyEffect;
    protected override void Start()
    {

    }

    protected override void Update()
    {
        transform.position += speed * Time.deltaTime;
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        GameObject tmpObj = Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        Destroy(tmpObj, tmpObj.GetComponentInChildren<ParticleSystem>().duration);
    }
}
