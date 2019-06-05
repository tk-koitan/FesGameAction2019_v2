using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;
using DG.Tweening;
using TadaLib;

namespace RocketStage
{
    public class LastBossController : MonoBehaviour
    {
        // === 外部パラメータ =============================================
        public float initHpMax = 50.0f;
        public float initSpeed = 6.0f;

        [System.NonSerialized] public float hpMax = 50.0f;
        [System.NonSerialized] public float hp = 50.0f;
        [System.NonSerialized] public float speed = 6.0f;
        [System.NonSerialized] public bool activeSts = false;

        [System.NonSerialized] public bool attackEnabled = false;
        [System.NonSerialized] public int attackDamage = 1;

        [SerializeField]
        private MeteoCreater meteoCreater;

        // === 内部パラメータ =============================================
        private float speedVx = 0.0f;
        private float speedVy = 0.0f;
        private float dir = 1.0f;

        // === キャッシュ =================================================
        [System.NonSerialized] public Animator animator;

        // メテオ関係
        [SerializeField]
        private GameObject meteoObject;

        public float normalMeteoInterval = 2.0f;
        public float homingMeteoInterval = 1.0f;
        public float posXMax = 11.0f;
        
        private Timer normalMeteoTimer;
        private Timer homingMeteoTimer;

        private AudioSource audioSource;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

            activeSts = true;

            hpMax = initHpMax;
            hp = hpMax;
            speed = initSpeed;

            normalMeteoTimer = new Timer(normalMeteoInterval);
            homingMeteoTimer = new Timer(homingMeteoInterval);
        }

        private void FixedUpdate()
        {
            //Debug.LogFormat("speedVx:{0}, speedVy{1}", speedVx, speedVy);
            transform.position += new Vector3(speedVx, speedVy, 0f) * Time.fixedDeltaTime;
        }

        public void MoveToDestination(Vector2 destination, float time)
        {
            if (time == 0)
            {
                Debug.Log("無効な引数値です");
                return;
            }
            speedVx = (destination.x - transform.position.x) / time;
            speedVy = (destination.y - transform.position.y) / time;
        }
        /*
        private IEnumerator MoveToDestinationCoroutine(Vector2 destination, float time)
        {
            speedVx = (destination.x - transform.position.x) / time;
            speedVy = (destination.y - transform.position.y) / time;

            yield return new WaitForSeconds(time);
        }*/

        public void EclipseMove()
        {
            if (transform.position.x >= posXMax) dir = -1f;
            else if (transform.position.x <= -posXMax) dir = 1f;

            speedVy = 0f;
            speedVx = speed * dir;
        }

        public void NormalMeteoAttack()
        {
            //Debug.Log(Time.fixedDeltaTime);
            normalMeteoTimer.TimeUpdate(Time.fixedDeltaTime);
            if (normalMeteoTimer.IsTimeout())
            {
                meteoCreater.MeteoSporne(posXMax, false);
                normalMeteoTimer.TimeReset();
            }
        }

        public void HomingMeteoAttack()
        {
            //Debug.Log(Time.fixedDeltaTime);
            homingMeteoTimer.TimeUpdate(Time.fixedDeltaTime);
            if (homingMeteoTimer.IsTimeout())
            {
                meteoCreater.MeteoSporne(posXMax, true);
                homingMeteoTimer.TimeReset();
            }
        }

        public void ActionMove(float nx, float ny)
        {
            if (nx != 0f)
            {
                speedVx = speed * nx;
                // animator.SetTrigger("Run");
            }
            else
            {
                speedVx = 0.0f;
                // animator.SetTrigger("Idle");
            }

            speedVy = speed * ny;
        }


        public void ActionDamage()
        {
            int damage = 0;
            if (hp <= 0)
            {
                return;
            }
            damage = 1;
            if (SetHp(hp - damage, hpMax))
            {
                Dead(false);
                // スコア加算
            }
        }

        public void Dead(bool gameOver)
        {
            if (!activeSts)
            {
                return;
            }
            activeSts = false;
            // animator.SetTrigger("Dead");
        }

        public bool SetHp(float _hp, float _hpMax)
        {
            hp = _hp;
            hpMax = _hpMax;
            return (hp <= 0);
        }
    }
}