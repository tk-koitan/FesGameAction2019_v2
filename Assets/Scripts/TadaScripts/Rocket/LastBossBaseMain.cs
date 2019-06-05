using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;

namespace RocketStage
{
    public enum ENEMYAISTS // --- 敵のAIステート ---
    {
        ACTIONSELECT, // アクション選択(思考)
        WAIT,         // 一定時間(止まって)待つ
        NORMALACTION,  // 楕円状に移動して、ロケットを発射
        ATTACK_A,       // プレイヤーの真上に移動して急降下
        ATTACK_B,       // プレイヤーに隕石を集中砲火
        TURN,         // その場で回転する
        FREEZ,        // 行動停止(ただし移動処理は継続する)
    }

    public class LastBossBaseMain : MonoBehaviour
    {
        // === 外部パラメータ(Inspector) ==================
        public int debug_SelectRandomAIState = -1;
        [SerializeField]
        protected BaseRocketController player;

        // === 外部パラメータ =============================
        [System.NonSerialized] public ENEMYAISTS aiState = ENEMYAISTS.ACTIONSELECT;

        // === キャッシュ =================================
        protected LastBossController lastBossCtrl;

        // === 内部パラメータ =============================
        protected float aiActionTimeLength = 0.0f;
        protected float aiActionTimeStart = 0.0f;
        protected float distanceToPlayer = 0.0f;
        protected float distanceToPlayerPrev = 0.0f;

        // =================================================
        public virtual void Awake()
        {
            lastBossCtrl = GetComponent<LastBossController>();
        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void FixedUpdate()
        {
            if (BeginEnemyCommonWork())
            {
                FixedUpdateAI();
                EndEnemyCommonWork();
            }
        }

        public virtual void FixedUpdateAI()
        {

        }

        // === 基本AI動作処理 ===============================
        public bool BeginEnemyCommonWork()
        {
            // 生きているか
            if (lastBossCtrl.hp <= 0)
            {
                return false;
            }

            lastBossCtrl.animator.enabled = true;

            // 状態チェック
            if (!CheckAction())
            {
                return false;
            }

            return true;
        }

        public void EndEnemyCommonWork()
        {
            // アクションのリミット時間をチェック
            float time = Time.fixedTime - aiActionTimeStart;
            if (time > aiActionTimeLength)
            {
                aiState = ENEMYAISTS.ACTIONSELECT;
            }
        }

        public bool CheckAction()
        {
            // 状態チェック
            AnimatorStateInfo stateInfo =
                lastBossCtrl.animator.GetCurrentAnimatorStateInfo(0);

            return true;
        }

        public int SelectRandomAIState()
        {
#if UNITY_EDITOR
            if (debug_SelectRandomAIState >= 0)
            {
                return debug_SelectRandomAIState;
            }
#endif
            return Random.Range(0, 100 + 1);
        }

        public void SetAIState(ENEMYAISTS sts, float t)
        {
            aiState = sts;
            aiActionTimeStart = Time.fixedTime;
            aiActionTimeLength = t;
        }

        public virtual void SetCombatAIState(ENEMYAISTS sts)
        {
            aiState = sts;
            aiActionTimeStart = Time.fixedTime;
            // enemyBirdCtrl.ActionMove(0f,0f);
        }

        // === AIスクリプトサポート関数 ========================
        public float GetDistancePlayer()
        {
            distanceToPlayerPrev = distanceToPlayer;
            distanceToPlayer = Vector3.Distance(
                transform.position, player.transform.position);
            return distanceToPlayer;
        }

        public bool IsChangeDistancePlayer(float l)
        {
            return (Mathf.Abs(distanceToPlayer - distanceToPlayerPrev) > l);
        }

        public float GetDistancePlayerX()
        {
            return (Mathf.Abs(transform.position.x - player.transform.position.x));
        }

        public float GetDistancePlayerY()
        {
            return (Mathf.Abs(transform.position.y - player.transform.position.y));
        }
    }
}