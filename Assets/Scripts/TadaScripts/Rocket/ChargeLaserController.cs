﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;

namespace RocketStage {
    public class ChargeLaserController : LaserController
    {
        [SerializeField]
        private ParticleSystem breakEffect;
        [SerializeField]
        private ParticleSystem meteoBreakEffect;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "DeadTrigger")
            {
                //Destroy(collision.gameObject);
                collision.gameObject.SetActive(false);
                Instantiate(breakEffect.gameObject, transform.position, Quaternion.identity);
                Instantiate(meteoBreakEffect.gameObject, collision.transform.position, Quaternion.identity);
            }
            else if (collision.tag == "LastBoss")
            {
                collision.GetComponent<Animator>().SetTrigger("Damage");
                Instantiate(breakEffect.gameObject, transform.position, Quaternion.identity);
                // 敵のHPを減らす
                collision.GetComponent<LastBossController>().ActionDamage(5);
            }
        }
    }
}