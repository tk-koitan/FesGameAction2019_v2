using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPatericle : MonoBehaviour
{
    Transform playerTrfm;
    public float distanceFromPlayer = 30.0f;
    private float wingAngle = 0.0f;

    private Joycon m_joyconR;

    // Start is called before the first frame update
    void Start()
    {
        playerTrfm = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_joyconR == null)
        {
            var joycons = JoyconManager.Instance.j;
            m_joyconR = joycons.Find(c => !c.isLeft);
            return;
        }

        Vector3 orientation = m_joyconR.GetVector().eulerAngles;
        Vector3 angles = transform.localEulerAngles;
        angles.z = orientation.y;
        transform.localEulerAngles = -angles + new Vector3(0f, 0f, 90f);

        wingAngle = transform.localRotation.eulerAngles.z;
        transform.position = new Vector3(playerTrfm.position.x + distanceFromPlayer * Mathf.Cos(wingAngle / 180f * Mathf.PI),
            distanceFromPlayer * Mathf.Sin(wingAngle / 180f * Mathf.PI), 0f);
        //transform.Rotate(0.5f, 0f, 0f);

        if (m_joyconR.GetButtonDown(Joycon.Button.SHOULDER_2))
        {
            m_joyconR.Recenter();
        }
    }
}
