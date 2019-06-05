using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;
using TadaLib;
using UnityEngine.UI;

namespace RocketStage {
    public class ShootingRocketController : BaseRocketController
    {
        [SerializeField]
        private GameObject normalLaser;
        [SerializeField]
        private GameObject chargeLaser;
        [SerializeField]
        private GameObject chargeEffect;
        [SerializeField]
        private GameObject chargeMaxEffect;
        [SerializeField]
        private Transform laserMuzzle;
        [SerializeField]
        private ParticleSystem normalLaserFlash;
        [SerializeField]
        private ParticleSystem chargeLaserFlash;

        public int hp = 10;
        public int hpMax = 10;

        [SerializeField]
        private Image[] hpImages;

        public float chargeSpeed = 1.0f;
        public float chargeMax = 4.0f;
        private float charge = 0.0f;
        public float laserInterval = 0.5f;

        public float chargeLaserBorder = 2.0f;

        [SerializeField]
        private AudioClip normalLaserSE;
        [SerializeField]
        private AudioClip chargeLaserSE;

        Timer laserTimer;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            // hpをコインの数にする
            // hpMax = coinNum;
            hp = hpMax;
            // hp外の画像を消す
            DeleteHpImage(30);

            laserTimer = new Timer(laserInterval);
        }

        // Update is called once per frame
        protected override void Update()
        {
            Debug.Log("hp : " + hp + " enabled : " + actionEnabled);
            base.Update();

            if (!actionEnabled) return;

            laserTimer.TimeUpdate(Time.deltaTime);

            if (!laserTimer.IsTimeout()) return;

            if (ActionInput.GetButton(ButtonCode.Jump))
            {
                if (!chargeEffect.activeSelf)
                    chargeEffect.SetActive(true);
                charge = Mathf.Min(charge + chargeSpeed * Time.deltaTime, chargeMax);
                if (charge >= chargeLaserBorder && !chargeMaxEffect.activeSelf)
                    chargeMaxEffect.SetActive(true);
            }
            else if (ActionInput.GetButtonUp(ButtonCode.Jump))
            {
                if (charge >= chargeLaserBorder)
                {
                    CreateChargeLaser();
                    chargeMaxEffect.SetActive(false);
                }
                else
                    CreateNormalLaser();
                charge = 0f;
                chargeEffect.SetActive(false);
                laserTimer.TimeReset();
            }
        }

        private void CreateNormalLaser()
        {
            GameObject laserObj = Instantiate(normalLaser, laserMuzzle.position, Quaternion.identity);
            laserObj.GetComponent<LaserController>().dir = transform.eulerAngles.z;
            normalLaserFlash.Play();
            audioSource.PlayOneShot(normalLaserSE);
        }

        private void CreateChargeLaser()
        {
            GameObject laserObj = Instantiate(chargeLaser, laserMuzzle.position, Quaternion.identity);
            laserObj.GetComponent<LaserController>().dir = transform.eulerAngles.z;
            laserObj.GetComponent<LaserController>().speed = charge;
            laserObj.GetComponent<LaserController>().defaultScale = 2 * charge / chargeMax;
            chargeLaserFlash.Play();
            audioSource.PlayOneShot(chargeLaserSE);
        }

        // ダメージを受ける
        public void ActionDamage(int damage)
        {
            if (hp <= 0)
            {
                return;
            }

            if (SetHp(hp - damage, hpMax))
            {
                Dead(false);
            }
            DeleteHpImage(hp + damage);
        }

        public void Dead(bool gameOver)
        {
            if (isDead)
            {
                return;
            }
            explosionEffect.Play();
            isDead = true;
            // animator.SetTrigger("Dead");
        }

        public bool SetHp(int _hp, int _hpMax)
        {
            hp = _hp;
            hpMax = _hpMax;
            return (hp <= 0);
        }

        private void DeleteHpImage(int hpBefore)
        {
            for(int i = Mathf.Max(hp, 0); i < hpBefore; i++)
            {
                hpImages[i].gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "DeadTrigger")
            {
                ActionDamage(1);
            }
        }
    }
}