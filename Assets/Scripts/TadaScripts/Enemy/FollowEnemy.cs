using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour
{

    [SerializeField]
    private GameObject attackBullet;

    public float posXfromPlayer = 1.0f;
    public float attackInterval = 3.0f;

    private float chargeTime = 0.0f;
    private bool isMoving = true;

    private Transform playerTrfm;

    private Joycon m_joyconR;
    private Rigidbody2D rb;
    private float startAngle;
    private ActionInput actionInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startAngle = transform.localEulerAngles.z;
        Debug.Log(startAngle);
        actionInput = ActionInput.Instatnce;

        playerTrfm = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position = new Vector3(playerTrfm.position.x + posXfromPlayer, transform.position.y, transform.position.z);
            chargeTime += Time.deltaTime;
            if(chargeTime >= attackInterval)
            {
                Attack();
                chargeTime %= attackInterval;
            }
        }

        // ジョイコンで回転させる
        rb.MoveRotation(actionInput.GetJoyconAngle() + startAngle);

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
    }

    private void Attack()
    {
        //Debug.Log("attack");
        GameObject waterStrObject = Instantiate(attackBullet,
        transform.position, Quaternion.identity);

        // 後ろ側を向いていてもプレイヤーの方へ撃たせる
        waterStrObject.GetComponent<BeamController>().dir = actionInput.GetJoyconAngle() + startAngle;
    }
}
