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
        }

        private void SetLeftDistance()
        {
            float moveDistance = speedVy;

            if (moveDistance < 0) moveDistance *= 0.1f;

            leftDistance -= (int)speedVy;
        }
    }
}