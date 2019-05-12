using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;

namespace RocketStage
{
    public class RollText : MonoBehaviour
    {
        public float speed = 10.0f;
        public float bottomY = -452f;
        public float topY = 453f;

        private float posY;
        private float posX;

        private void Start()
        {
            posY = transform.localPosition.y;
            posX = transform.localPosition.x;
        }
        // Update is called once per frame
        void Update()
        {
            posY -= speed;
            if (posY < bottomY)
            {
                posY = topY - (bottomY - posY);
            }
            transform.localPosition = new Vector3(posX, posY);
        }
    }
}