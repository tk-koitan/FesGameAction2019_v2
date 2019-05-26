using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;
using TadaLib;

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
        private Transform laserMuzzle;
        [SerializeField]
        private ParticleSystem normalLaserFlash;
        [SerializeField]
        private ParticleSystem chargeLaserFlash;

        public float chargeSpeed = 1.0f;
        public float chargeMax = 4.0f;
        private float charge = 0.0f;
        public float laserInterval = 0.5f;

        public float chargeLaserBorder = 2.0f;

        Timer laserTimer;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            laserTimer = new Timer(laserInterval);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            if (!actionEnabled) return;

            laserTimer.TimeUpdate(Time.deltaTime);

            if (!laserTimer.IsTimeout()) return;

            if (ActionInput.GetButton(ButtonCode.Jump))
            {
                if (!chargeEffect.activeSelf)
                    chargeEffect.SetActive(true);
                charge = Mathf.Min(charge + chargeSpeed * Time.deltaTime, chargeMax);
            }
            else if (ActionInput.GetButtonUp(ButtonCode.Jump))
            {
                if (charge >= chargeLaserBorder)
                    CreateChargeLaser();
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
        }

        private void CreateChargeLaser()
        {
            GameObject laserObj = Instantiate(chargeLaser, laserMuzzle.position, Quaternion.identity);
            laserObj.GetComponent<LaserController>().dir = transform.eulerAngles.z;
            laserObj.GetComponent<LaserController>().speed = charge;
            laserObj.GetComponent<LaserController>().defaultScale = 2 * charge / chargeMax;
            chargeLaserFlash.Play();
        }
    }
}