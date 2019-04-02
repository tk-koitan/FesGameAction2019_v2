using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "CameraTrigger")
        {
            Camera.main.GetComponent<CameraFollow>().SetCamera(
                collision.GetComponent<StageTrigger_Camera>().param); // カメラの情報をセット
        }

        if(collision.tag == "DropTrigger")
        {
            GetComponent<PlayerRB>().Retry();
        }

        if(collision.tag == "DeadTrigger")
        {
            GetComponent<PlayerRB>().StartDeadAnimtion();
        }
    }
}
