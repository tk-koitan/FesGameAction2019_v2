using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;

namespace RocketStage
{
    public class RocketBody : MonoBehaviour
    {
        RocketController rocketCtrl;

        private void Start()
        {
            rocketCtrl = GetComponent<RocketController>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "DeadTrigger")
            {
                rocketCtrl.isDead = true;
            }
        }
    }
}