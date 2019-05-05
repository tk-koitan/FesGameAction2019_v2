using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;

public class Snake : MonoBehaviour
{
    public GameObject bullet;
    public GameObject target;
    public float bulletSpeed;
    public float radius;
    public float interval;
    private float intervalTime;
    // Start is called before the first frame update
    void Start()
    {
        intervalTime = interval;
    }

    // Update is called once per frame
    void Update()
    {
        if ((target.transform.position - transform.position).magnitude < radius)
        {
            intervalTime -= Time.deltaTime;
        }
        else
        {
            intervalTime = interval;
        }
        if (intervalTime < 0)
        {
            intervalTime += interval;
            GameObject tmpObj = Instantiate(bullet, transform.position, Quaternion.identity);
            tmpObj.GetComponent<Bullet>().speed = (target.transform.position - transform.position).normalized * bulletSpeed;
        }
    }

    private void OnDrawGizmos()
    {
        GizmosExtensions2D.DrawWireCircle2D(transform.position, radius);
        if ((target.transform.position - transform.position).magnitude < radius)
        {
            GizmosExtensions2D.DrawArrow2D(transform.position, target.transform.position);
        }
    }
}
