using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;

namespace RocketStage
{
    public class RocketBackGround : MonoBehaviour
    {
        [SerializeField]
        private RocketController rocket;

        public float speed = 20.0f;
        private float posY;
        public float height = 16.0f;

        // Start is called before the first frame update
        void Start()
        {
            posY = transform.position.y + height;
        }

        // Update is called once per frame
        void Update()
        {
            speed = rocket.speedVy * 10f;
            posY += speed * Time.deltaTime;
            posY = (posY + height * 2.0f) % (height * 2.0f);
            transform.position = new Vector3(0f, -posY + height, 0f);
        }
    }
}