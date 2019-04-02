using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KoitanLib;
using UnityEditor;
using System.Linq;

public class JoyconRotator : Mover
{

    private Joycon m_joyconR;
    private Rigidbody2D rb;
    private float startAngle;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        startAngle = transform.localEulerAngles.z;
        Debug.Log(startAngle);
    }

    protected override void Update()
    {
        base.Update();
        if(m_joyconR == null)
        {
            var joycons = JoyconManager.Instance.j;
            m_joyconR = joycons.Find(c => !c.isLeft);
            return;
        }
        var orientation = m_joyconR.GetVector().eulerAngles;
        var angles = transform.localEulerAngles;
        angles.z = orientation.y + startAngle;
        //transform.localEulerAngles = angles;

        rb.MoveRotation(orientation.y + startAngle);

        if (m_joyconR.GetButtonDown(Joycon.Button.SHOULDER_2))
        {
            m_joyconR.Recenter();
        }
    }
    // Update is called once per frame
    /*
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
    */
}
