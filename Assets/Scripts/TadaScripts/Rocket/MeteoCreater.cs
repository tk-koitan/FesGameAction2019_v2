using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;

namespace RocketStage
{
    public class MeteoCreater : MonoBehaviour
    {
        [SerializeField]
        private GameObject meteoObject;

        [SerializeField]
        private Transform playerTrfm;

        [SerializeField]
        private AudioClip meteoSE;
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void MeteoSporne(float posXRange, bool isHoming = false)
        {
            //Debug.Log("drop");
            float posX = Random.Range(-posXRange, posXRange);

            var pool = Pool.GetObjectPool(meteoObject);
            GameObject obj = pool.GetInstance();

            if (obj != null)
            {
                audioSource.PlayOneShot(meteoSE);
                obj.transform.position = new Vector3(posX,
                    12f, transform.position.z);
                if (isHoming)
                {
                    obj.GetComponent<MeteoDrop>().SetHoming(playerTrfm.position);
                }
            }
        }

        public void MeteoSporne(float posX, float dir)
        {
            var pool = Pool.GetObjectPool(meteoObject);
            GameObject obj = pool.GetInstance();

            if (obj != null)
            {
                audioSource.PlayOneShot(meteoSE);
                obj.transform.position = new Vector3(posX * dir,
                    transform.position.y, transform.position.z);
                obj.GetComponent<MeteoDrop>().v = new Vector3(0f, -Mathf.Max((9 - posX / 1.4f), 3.0f), 0f);
                obj.GetComponent<MeteoDrop>().gravity = -0.5f;
            }
        }
    }
}