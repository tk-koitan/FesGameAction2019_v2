using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyconUmbrella : Mover
{
    private Joycon m_joyconR;
    private float startAngle;
    public Transform target;
    private Vector3 offset;
    public Transform tuka;
    public Transform body;

    protected override void Start()
    {
        base.Start();
        startAngle = transform.rotation.eulerAngles.z;
        offset = transform.position - target.position;
    }

    protected override void Update()
    {
        base.Update();
        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, ActionInput.GetJoyconVector().y + startAngle));
        //rb.MoveRotation(ActionInput.GetJoyconVector().y + startAngle);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeadTrigger")
        {
            Destroy(collision.gameObject);
        }
    }
}
