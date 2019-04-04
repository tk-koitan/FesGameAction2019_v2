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
    private ActionInput actionInput;
    private Vector2 targetPos;
    public float kakerukazu = 0.9f;

    protected override void Start()
    {
        base.Start();
        defaultPos = currentPos;
        actionInput = ActionInput.Instatnce;
    }

    protected override void Update()
    {
        float speed = actionInput.GetJoyconAccel().magnitude;
        targetPos = defaultPos + Vector2.up * speed;
        transform.position += ((Vector3)targetPos - transform.position) * kakerukazu;
        base.Update();
        if(speed>5)
        {
            foreach(PlayerRB player in ridingPlayers)
            {
                player.power.y = speed * 2;
                player.isGround = false;
            }
        }
    }
    // Update is called once per frame
    /*
    protected override void Update()
    {
        base.Update();
    }
    */

    public override Vector2 Velocity
    {
        get
        {
            return v;
        }
    }
}
