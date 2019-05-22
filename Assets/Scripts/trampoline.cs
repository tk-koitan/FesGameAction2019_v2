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
    private Vector3 defaultPos;
    private ActionInput actionInput;
    private Vector3 targetPos;
    public float kakerukazu = 0.9f;
    private float startAngle;
    public float maxSpeed = 3;

    private AudioSource audioSource;
    private Renderer targetRenderer;

    protected override void Start()
    {
        base.Start();
        defaultPos = currentPos;
        startAngle = transform.rotation.eulerAngles.z;
        actionInput = ActionInput.Instatnce;
        audioSource = GetComponent<AudioSource>();
        targetRenderer = GetComponent<Renderer>();
    }

    protected override void Update()
    {
        float speed = ActionInput.GetJoyconAccel().magnitude;
        if (speed < 1.2f) speed = 1f;
        speed = Mathf.Min(speed, maxSpeed);
        targetPos = defaultPos + transform.up * (speed - 1);
        transform.position += (targetPos - transform.position) * kakerukazu;
        base.Update();
        if (speed >= maxSpeed)
        {
            foreach (PlayerRB player in ridingPlayers)
            {
                player.power = transform.up * maxSpeed;
                player.isGround = false;
                audioSource.PlayOneShot(audioSource.clip);
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
