using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KoitanLib;
using UnityEditor;
using System.Linq;

public class trampoline : Mover
{

    private Joycon m_joyconR;
    private Vector2 defaultPos;

    protected override void Start()
    {
        base.Start();
        defaultPos = currentPos;
    }

    private void Update()
    {
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

        transform.position = defaultPos + Vector2.up * m_joyconR.GetAccel().magnitude;
    }
    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override Vector2 Velocity
    {
        get
        {
            return v;
        }
    }
}
