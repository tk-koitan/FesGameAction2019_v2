using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;

namespace RocketStage {
    public class ShootingRocketController : BaseRocketController
    {
        [SerializeField]
        private GameObject laser;

        public float chargeSpeed = 1.0f;
        public float chargeMax = 10.0f;
        private float charge = 0.0f;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            if (!actionEnabled) return;

            charge = Mathf.Min(charge + chargeSpeed * Time.deltaTime, chargeMax);
            if (ActionInput.GetButtonDown(ButtonCode.Jump))
            {
                CreateLaser();
                charge = 0f;
            }
        }

        private void CreateLaser()
        {
            GameObject laserObj = Instantiate(laser, transform.position, Quaternion.identity);
            laserObj.GetComponent<LaserController>().dir = transform.eulerAngles.z;
            laserObj.GetComponent<LaserController>().speed = charge;
            laserObj.GetComponent<LaserController>().defaultScale = charge / chargeMax;
        }
    }
}