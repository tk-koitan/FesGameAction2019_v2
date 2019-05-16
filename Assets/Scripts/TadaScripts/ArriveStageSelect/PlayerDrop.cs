using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DropRocketScene;
using TadaLib;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace DropRocketScene
{
    public class PlayerDrop : MonoBehaviour
    {
        [SerializeField]
        private CameraShake cam;

        public Transform destination;

        public float arriveTime = 3.0f;
        public float nextSceneTimeFromArrive = 3.0f;
        //public float upHeight = 5.0f;

        public int rotateLevel;

        public float gravity = -9.8f;

        private float rotate;

        private Vector3 initVelocity;
        private Vector3 addVelocity = Vector3.zero;

        private bool actionEnabled = true;
        Timer timer, sceneTimer;

        // Start is called before the first frame update
        void OnEnable()
        {
            timer = new Timer(arriveTime);
            sceneTimer = new Timer(arriveTime + nextSceneTimeFromArrive);
            rotate = 360f / arriveTime * (float)rotateLevel;
            initVelocity = (destination.position - transform.position) / arriveTime;
            float time = arriveTime / 2f;
            addVelocity.y = -gravity * time; // upHeight / time - (gravity / 2f) * time;
            GetComponent<Animator>().SetTrigger("Death");

            // カメラを揺らす
            SetCameraShake(true);
        }

        // Update is called once per frame
        void Update()
        {
            sceneTimer.TimeUpdate(Time.deltaTime);

            if (sceneTimer.IsTimeout())
                SceneManager.LoadScene("KawazStageSelect");

            if (!actionEnabled) return;

            if (timer.IsTimeout())
            {
                SetCamera();
                SetCameraShake(false);
                actionEnabled = false;
                return;
            }

            transform.Rotate(0f, 0f, rotate * Time.deltaTime);

            transform.position += (initVelocity + addVelocity) * Time.deltaTime;

            VyUpdate();
            timer.TimeUpdate(Time.deltaTime);
        }

        private void VyUpdate()
        {
            addVelocity.y += gravity * Time.deltaTime;
        }

        private void SetCameraShake(bool isShake)
        {
            cam.isShake = isShake;
        }

        private void SetCamera()
        {
            DOTween.To(
                () => Camera.main.orthographicSize,
                num => Camera.main.orthographicSize = num,
                8.0f,
                2.0f);

            DOTween.To(
                () => Camera.main.transform.position,
                num => Camera.main.transform.position = num,
                new Vector3(transform.position.x, transform.position.y, -10f),
                2.0f);
        }
    }
}