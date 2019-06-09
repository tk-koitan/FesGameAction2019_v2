using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using RocketStage;

namespace RocketStage
{
    public class RocketController : BaseRocketController
    {
        [SerializeField]
        private HukidashiController hukidashiR;
        [SerializeField]
        private HukidashiController hukidashiL;

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            if (!actionEnabled) return;

            SetLeftDistance();

            if (isDead)
                GoNextScene();

            if (ActionInput.GetButtonDown(ButtonCode.Cancel))
            {
                if(!hukidashiL.isExist)
                    hukidashiL.FlowStart();
            }
            if (ActionInput.GetButtonDown(ButtonCode.Jump))
            {
                if (!hukidashiR.isExist)
                    hukidashiR.FlowStart();
            }
        }

        private void SetLeftDistance()
        {
            float moveDistance = speedVy;

            if (moveDistance < 0) moveDistance *= 0.1f;

            leftDistance -= (int)speedVy;
        }
    }
}