using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageSelect;

namespace StageSelect
{
    public class StageSelectPlayer : MonoBehaviour
    {

        // 面倒だからあとで

        /*
    public float speed = 15.0f;

    private float defaultScaleX;

    private Vector2 prevPos;

    private float dir;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        dir = (transform.localScale.x > 0.0f) ? 1 : -1; // tada
        defaultScaleX = transform.localScale.x * dir;
        transform.localScale = new Vector3(
            defaultScaleX, transform.localScale.y, transform.localScale.z);

        animator = GetComponent<Animator>();
        prevPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.x - prevPos.x > 0) dir = 1f;
        else if (transform.position.x - prevPos.x < 0) dir = -1f;
        //animator.SetFloat("MoveSpeed", (isMoving) ? 10 : 0);
        transform.localScale = new Vector3(
            defaultScaleX * dir, transform.localScale.y, transform.localScale.z);

        prevPos = transform.position;
    }*/
    }
}