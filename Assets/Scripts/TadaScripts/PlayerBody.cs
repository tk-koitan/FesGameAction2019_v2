using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBody : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "CameraTrigger")
        {
            Camera.main.GetComponent<CameraFollow>().SetCamera(
                collision.GetComponent<StageTrigger_Camera>().param); // カメラの情報をセット
        }

        if(collision.tag == "DeadTrigger")
        {
            Debug.Log("死亡しました");
        }
    }
}
