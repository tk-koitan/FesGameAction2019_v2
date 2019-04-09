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
    private ActionInput actionInput;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        startAngle = transform.localEulerAngles.z;
        Debug.Log(startAngle);
        actionInput = ActionInput.Instatnce;
    }

    protected override void Update()
    {
        base.Update();

        rb.MoveRotation(ActionInput.GetJoyconVector().y + startAngle);

        /*
        if (m_joyconR == null)
        {
            var joycons = JoyconManager.Instance.j;
            m_joyconR = joycons.Find(c => !c.isLeft);
            return;
        }

        if (m_joyconR.GetButtonDown(Joycon.Button.SHOULDER_2))
        {
            m_joyconR.Recenter();
        }
        */
    }
    // Update is called once per frame
    /*
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
    */
}
