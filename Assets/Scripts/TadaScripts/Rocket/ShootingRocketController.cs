using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;
using TadaLib;
using DG.Tweening;

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
        private PlayerHPController[] hpImages;

        public float chargeSpeed = 1.0f;
        public float chargeMax = 4.0f;
        private float charge = 0.0f;
        public float laserInterval = 0.5f;

        public float chargeLaserBorder = 2.0f;

        public float hpDisplayTime = 3.0f;

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

            laserTimer = new Timer(laserInterval);
        }

        // Update is called once per frame
        protected override void Update()
        {
            //Debug.Log("hp : " + hp + " enabled : " + actionEnabled);
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

            if (isDead) GameOver();
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
            if (!actionEnabled) return;

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
            GameOver();
            isDead = true;
            // animator.SetTrigger("Dead");
        }

        private void GameOver()
        {
            GoNextScene();
            //FadeManager.Instance.LoadScene("KawazStageSelect", 2.0f);
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
                hpImages[i].DestroyCoin();
            }
        }

        public void ShowHpImage()
        {
            // hp分コインを表示させる
            StartCoroutine(ShowHpImage(hp, hpDisplayTime));
        }

        private IEnumerator ShowHpImage(int num, float time)
        {
            for(int i = 0; i < num; i++)
            {
                hpImages[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(time / (float)num);
            }
        }

        // クリア後に出て行くアニメーション
        public void DoEndAnimation()
        {
            actionEnabled = false;

            /*DOTween.To(
                () => transform.eulerAngles,
                num => transform.eulerAngles = num,
                new Vector3(0f, 0f, 0f),
                startAnimationTime / 2f);
                */
            transform.DOMoveY(
                12.0f,
                startAnimationTime);
        }
        /*
        private IEnumerator EndAnimation()
        {
            actionEnabled = false;

            DOTween.To(
                () => transform.eulerAngles,
                num => transform.eulerAngles = num,
                new Vector3(0f, 0f, 0f),
                startAnimationTime / 2f);

            transform.DOMoveY(
                1.0f,
                startAnimationTime);

            yield return new WaitForSeconds(startAnimationTime);
            //actionEnabled = true;
        }*/

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "DeadTrigger")
            {
                ActionDamage(1);
                collision.GetComponent<MeteoDrop>().DestroyMeteo();
            }
            if(collision.tag == "LastBoss")
            {
                ActionDamage(2);
            }
        }
    }
}