using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyConDemo : MonoBehaviour
{
    Joycon m_joyconR;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        // ジョイコンの親オブジェクトを画面左上に固定
        transform.position = new Vector3(cam.transform.position.x - cam.orthographicSize * cam.aspect * 0.6f,
            cam.transform.position.y + cam.orthographicSize * 0.6f, 0.0f);
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

        //Quaternion orientation = m_joyconR.GetVector();
        //orientation = new Quaternion(orientation.w, orientation.y, orientation.z, orientation.x);

        // eulerAnglesで3次元に変換してから
        Vector3 orientation = m_joyconR.GetVector().eulerAngles;
        Vector3 angles = transform.localEulerAngles;
        //angles.x = -orientation.x;
        //angles.y = orientation.z + 180;
        angles.z = orientation.y + 180;
        //Debug.LogFormat("x:{0}, y:{1}, z:{2}", orientation.x, orientation.z + 180, orientation.y + 180);
        //transform.localEulerAngles = angles;
        

         //そのままQuartanionで
        //Quaternion orientation = m_joyconR.GetVector();
        //orientation = new Quaternion(orientation.w, orientation.y, orientation.z, orientation.x);
        

        // ジャイロで
        //Vector3 gyro = m_joyconR.GetGyro();
        //Debug.Log(gyro);

        foreach (Transform child in transform)
        {
            // ジョイコンの傾きを同期させる
            //child.transform.rotation = orientation;
            child.transform.localEulerAngles = angles;

            //child.transform.Rotate(-gyro.y * 1f, gyro.x * 1f, -gyro.z * 1f);
        }

    }
}
