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
        [System.NonSerialized] public bool isDead = false;

        [System.NonSerialized] public int attackDamage = 1;

        public bool actionEnabled = false;
        public float animationSpeed = 1.0f;

        [SerializeField]
        private MeteoCreater meteoCreater;
        [SerializeField]
        private Transform[] eclipsePath;

        [SerializeField]
        protected ParticleSystem explosionEffect;

        // === 内部パラメータ =============================================
        private float speedVx = 0.0f;
        private float speedVy = 0.0f;
        private float dir = 1.0f;

        // === キャッシュ =================================================
        [System.NonSerialized] public Animator animator;

        Tween moveTween;

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
            if (!actionEnabled) return;
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

        public void EclipseMove(float time)
        {
            speedVx = 0f;
            speedVy = 0f;

            // 禁断のDOTWeen使います
            // 時計回りで楕円運動
            // 始めに第何象限にいるのかを確かめる
            int index = 0; // 左上から時計回りに0,1,2,3
            if (transform.position.x > 0f) index++;
            if (transform.position.y < 5f) index += 3 - index;

            Vector3[] path = new Vector3[eclipsePath.Length];
            for(int i = 0; i < eclipsePath.Length; i++)
            {
                path[i] = eclipsePath[(index + i) % eclipsePath.Length].transform.position;
            }

            moveTween = transform.DOPath(path, time, PathType.CatmullRom).SetEase(Ease.OutQuad);

            /*
            if (transform.position.x >= posXMax * 0.9f) dir = -1f;
            else if (transform.position.x <= -posXMax * 0.9f) dir = 1f;

            speedVy = 0f;
            speedVx = speed * dir;
            */
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

        // 一つの隕石を場所、角度を決めて落とす
        public void DropOneMeteo(float posX, float dir)
        {
            meteoCreater.MeteoSporne(posX, dir);
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


        public void ActionDamage(int damage)
        {
            if (hp <= 0)
            {
                return;
            }

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
            explosionEffect.transform.position = transform.position;
            explosionEffect.gameObject.SetActive(true);

            activeSts = false;
            isDead = true;
            moveTween.Kill();
            // animator.SetTrigger("Dead");
        }

        public bool SetHp(float _hp, float _hpMax)
        {
            hp = _hp;
            hpMax = _hpMax;
            return (hp <= 0);
        }

        // 出てくるときのアニメーション
        public void BeginAnimation()
        {
            transform.DOMoveY(
            4.0f,
            2.0f / animationSpeed);
        }

        // 出て行くときのアニメーション
        public void EndAnimation()
        {
            transform.DOMoveY(
            12.0f,
            2.0f / animationSpeed);
        }
    }
}