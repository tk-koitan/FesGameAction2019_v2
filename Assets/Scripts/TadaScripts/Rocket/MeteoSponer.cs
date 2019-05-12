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

        [SerializeField]
        private GameObject meteoObject;

        public float interval = 2.0f;
        public float intervalAccel = 0.02f;
        public float posXMax = 11.0f;

        private float time = 0.0f;

        public float delayTime = 3.0f;

        // Start is called before the first frame update
        void Start()
        {
            timer = new Timer(interval);
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

            if (timer.IsTimeout())
            {
                MeteoCreate();
                timer.TimeReset();
            }
        }

        void MeteoCreate()
        {
            float posX = Random.Range(-posXMax, posXMax);
            Instantiate(meteoObject, new Vector3(posX, transform.position.y, transform.position.z), Quaternion.identity);
        }
    }
}