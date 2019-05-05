using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KoitanLib;
using UnityEditor;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
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
        //Debug.Log(startAngle);
    }

    protected override void Update()
    {
        base.Update();

        rb.MoveRotation(ActionInput.GetJoyconVector().y + startAngle);
    }
}
