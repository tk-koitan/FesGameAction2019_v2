using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TadaLib;
using RocketStage;

namespace RocketStage
{
    public class MeteoSponer : MonoBehaviour
    {
        Timer timer;
        Timer endTimer;

        [SerializeField]
        private GameObject meteoObject;

        public float interval = 2.0f;
        public float intervalAccel = 0.02f;
        public float posXMax = 11.0f;

        private float time = 0.0f;

        public float delayTime = 3.0f;

        public bool isDeathComb = true;
        public float endTime = 30.0f;

        private bool endCombing = false;

        // Start is called before the first frame update
        void Start()
        {
            timer = new Timer(interval);
            endTimer = new Timer(endTime);
        }

        // Update is called once per frame
        void Update()
        {
            if (delayTime > 0)
            {
                delayTime -= Time.deltaTime;
                return;
            }

            time += Time.deltaTime;
            timer.TimeUpdate(Time.deltaTime + time * intervalAccel);
            endTimer.TimeUpdate(Time.deltaTime);

            if (endTimer.IsTimeout())
            {
                if(!endCombing)
                    StartCoroutine(BeginDeathComb(4.0f));
            }
            else if (timer.IsTimeout())
            {
                MeteoCreate();
                timer.TimeReset();
            }
        }

        void MeteoCreate()
        {
            float posX = Random.Range(-posXMax, posXMax);
            /*
            Instantiate(meteoObject, new Vector3(posX, transform.position.y, transform.position.z), Quaternion.identity);
            */

            var pool = Pool.GetObjectPool(meteoObject);
            GameObject obj = pool.GetInstance();

            if (obj != null)
            {
                obj.transform.position = new Vector3(posX,
                    transform.position.y, transform.position.z);
            }
        }

        private IEnumerator BeginDeathComb(float interval)
        {
            endCombing = true;

            // 左右から中央へ降らせる
            while (true)
            {
                for(float posX = 10f; posX >= 0f; posX -= 1.55f)
                {
                    for (int dir = -1; dir <= 1; dir += 2)
                    {
                        var pool = Pool.GetObjectPool(meteoObject);
                        GameObject obj = pool.GetInstance();

                        if (obj != null)
                        {
                            obj.transform.position = new Vector3(posX * dir,
                                transform.position.y, transform.position.z);
                            obj.GetComponent<MeteoDrop>().v = new Vector3(0f, -(9 - posX / 1.2f), 0f);
                            obj.GetComponent<MeteoDrop>().gravity = -0.5f;
                        }
                    }

                    yield return new WaitForSeconds(interval / 10f);
                }
                /*
                yield return new WaitForSeconds(interval / 20f);

                for (float posX = 0.7f ; posX <= 10f; posX += 1.55f)
                {
                    for (int dir = -1; dir <= 1; dir += 2)
                    {
                        var pool = Pool.GetObjectPool(meteoObject);
                        GameObject obj = pool.GetInstance();

                        if (obj != null)
                        {
                            obj.transform.position = new Vector3(posX * dir,
                                transform.position.y, transform.position.z);
                            obj.GetComponent<MeteoDrop>().v = new Vector3(0f, -(5.5f + posX / 1.4f), 0f);
                            obj.GetComponent<MeteoDrop>().gravity = -0.75f;
                        }
                    }

                    yield return new WaitForSeconds(interval / 10f);
                }*/
                yield return new WaitForSeconds(interval / 5f);
            }
        }
    }
}