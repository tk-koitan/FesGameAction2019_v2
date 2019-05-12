using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DropRocketScene;
using TadaLib;
using DG.Tweening;
using SpriteGlow;

namespace DropRocketScene
{
    public class RocketFlagment : MonoBehaviour
    {
        public Transform destination;

        public float arriveTime = 3.0f;
        //public float upHeight = 5.0f;

        public float rotateMax = 360.0f;
        public float rotateMin = 60.0f;

        public float gravity = -9.8f;

        private float rotate;

        private Vector3 initVelocity;
        private Vector3 addVelocity = Vector3.zero;

        private bool actionEnabled = true;
        Timer timer;

        // Start is called before the first frame update
        void OnEnable()
        {
            timer = new Timer(arriveTime);

            rotate = Random.Range(rotateMin, rotateMax);
            initVelocity = (destination.position - transform.position) / arriveTime;
            float time = arriveTime / 2f;
            addVelocity.y = -gravity * time; // upHeight / time - (gravity / 2f) * time;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0f, 0f, rotate * Time.deltaTime);

            if (!actionEnabled) return;

            if (timer.IsTimeout())
            {
                FeedOut();
                StageMarkReded();
                actionEnabled = false;
                return;
            }

            transform.position += (initVelocity + addVelocity) * Time.deltaTime;

            VyUpdate();
            timer.TimeUpdate(Time.deltaTime);
        }

        private void VyUpdate()
        {
            addVelocity.y += gravity * Time.deltaTime;
        }

        private void FeedOut()
        {
            DOTween.To(
                () => GetComponent<SpriteRenderer>().color,
                num => GetComponent<SpriteRenderer>().color = num,
                new Color(0f, 0f, 0f, 0f),
                2.0f).OnComplete(() => Destroy(gameObject));
        }

        private void StageMarkReded()
        {
            DOTween.To(
                () => destination.gameObject.GetComponent<SpriteRenderer>().color,
                num => destination.gameObject.GetComponent<SpriteRenderer>().color = num,
                new Color(1f, 0f, 0f, 1f),
                5.0f);

            destination.gameObject.GetComponent<SpriteGlowEffect>().OutlineWidth = 2;

            DOTween.To(
                () => destination.gameObject.GetComponent<SpriteGlowEffect>().GlowBrightness,
                num => destination.gameObject.GetComponent<SpriteGlowEffect>().GlowBrightness = num,
                8.0f,
                5.0f);
        }
    }
}