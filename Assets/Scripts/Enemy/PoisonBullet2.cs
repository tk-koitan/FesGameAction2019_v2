using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBullet2 : Bullet
{
    public Vector3 rotationSpeed;
    public GameObject DestroyEffect;
    public float gravity;
    protected override void Start()
    {

    }

    protected override void Update()
    {
        speed.y += gravity * Time.deltaTime;
        transform.position += speed * Time.deltaTime;
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "DeadTrigger")
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        GameObject tmpObj = Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        Destroy(tmpObj, tmpObj.GetComponentInChildren<ParticleSystem>().duration);
    }
}
