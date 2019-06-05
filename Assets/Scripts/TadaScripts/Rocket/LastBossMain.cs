﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;

namespace RocketStage
{
    public class LastBossMain : LastBossBaseMain
    {
        // === 外部パラメータ(Inspector) ======================
        public int aiIfNORMALACTION = 50;
        public int aiIfATTACK1 = 30;
        public int aiIfATTACK2 = 20;

        public int damageAttack_A = 1;

        // === 内部パラメータ =================================
        private bool isAttacking = false;

        // ====================================================
        public override void FixedUpdateAI()
        {
            // AIステート
            Debug.Log(string.Format(">>> aists{0}", aiState));
            switch (aiState)
            {
                case ENEMYAISTS.ACTIONSELECT: // 思考の起点
                                              // アクションの選択
                    int n = SelectRandomAIState();
                    if (n < aiIfNORMALACTION)
                    {
                        SetAIState(ENEMYAISTS.NORMALACTION, 5.0f);
                    }
                    else if (n < aiIfNORMALACTION + aiIfATTACK1)
                    {
                        SetAIState(ENEMYAISTS.ATTACK_A, 10.0f);
                    }
                    else if (n < aiIfNORMALACTION + aiIfATTACK1 + aiIfATTACK2)
                    {
                        SetAIState(ENEMYAISTS.ATTACK_B, 5.0f);
                    }
                    else
                    {
                        SetAIState(ENEMYAISTS.WAIT, 1.0f + Random.Range(0.0f, 1.0f));
                    }
                    lastBossCtrl.ActionMove(0.0f, 0.0f);
                    break;

                case ENEMYAISTS.WAIT: //休憩
                    lastBossCtrl.ActionMove(0.0f, 0.0f);
                    break;

                case ENEMYAISTS.ATTACK_A: // プレイヤーの真上にいって急降下
                    if(!isAttacking)
                        Attack_A();
                    break;

                case ENEMYAISTS.ATTACK_B: // プレイヤーに隕石を向けて大量発射
                    if (!isAttacking)
                        Attack_B();
                    break;

                case ENEMYAISTS.NORMALACTION: // 楕円状に回転して隕石を降らせる
                    NormalAction();
                    break;
            }
        }

        // === アクション処理 =================================================

        // プレイヤーの真上に行って急降下
        private void Attack_A()
        {
            StartCoroutine(Attack_ACoroutine());
        }
        // プレイヤーに隕石を狙い撃ち
        private void Attack_B()
        {
            StartCoroutine(Attack_BCoroutine());
        }
        // 楕円状に移動して隕石を相手に降らせる
        private void NormalAction()
        {
            // 隕石を振らせる
            lastBossCtrl.NormalMeteoAttack();
            // 楕円状に移動する
            lastBossCtrl.EclipseMove();
        }

        private IEnumerator Attack_ACoroutine()
        {
            isAttacking = true;

            // プレイヤーのx座標に移動
            float targetX = player.transform.position.x;
            float duration = GetDistancePlayerX() / (lastBossCtrl.speed * 2f);

            lastBossCtrl.MoveToDestination(new Vector2(targetX, transform.position.y), duration);

            yield return new WaitForSeconds(duration);

            // 少しだけ止まる
            lastBossCtrl.ActionMove(0f, 0f);

            yield return new WaitForSeconds(0.5f);

            // 急降下
            float targetY = -7f;
            duration = Mathf.Abs(transform.position.y - targetY) / (lastBossCtrl.speed * 2.0f);

            lastBossCtrl.MoveToDestination(new Vector2(transform.position.x, targetY), duration);

            yield return new WaitForSeconds(duration);

            // 戻る
            targetY = 5f;
            duration = Mathf.Abs(transform.position.y - targetY) / lastBossCtrl.speed;

            lastBossCtrl.MoveToDestination(new Vector2(transform.position.x, targetY), duration);

            yield return new WaitForSeconds(duration);

            isAttacking = false;
            SetAIState(ENEMYAISTS.WAIT, 1.0f);
        }

        private IEnumerator Attack_BCoroutine()
        {
            isAttacking = true;

            // 真ん中にいく
            float targetX = 0f;
            float duration = GetDistancePlayerX() / (lastBossCtrl.speed * 2f);

            lastBossCtrl.MoveToDestination(new Vector2(targetX, transform.position.y), duration);

            yield return new WaitForSeconds(duration);

            // 止まる
            lastBossCtrl.ActionMove(0f, 0f);

            // プレイヤーに隕石を狙い撃ち
            float time = 0f;
            while (true)
            {
                if (time >= 3f) break;
                time += Time.fixedDeltaTime;
                lastBossCtrl.HomingMeteoAttack();
                yield return new WaitForSeconds(0.001f);
            }

            isAttacking = false;
            SetAIState(ENEMYAISTS.WAIT, 1.0f);
        }

    }
}