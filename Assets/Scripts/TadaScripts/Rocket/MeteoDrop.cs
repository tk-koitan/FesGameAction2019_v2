using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;

namespace RocketStage
{
    public class MeteoDrop : MonoBehaviour
    {
        public float vxMax = 1.0f;
        public float vyMin = -2.0f;
        public float vyMax = -5.0f;

        public float gravityMin = -0.1f;
        public float gravityMax = -1.0f;

        public Vector3 v;
        public float gravity;

        public float lifeTime = 3.0f;

        public float borderX = 12.0f;
        public float borderY = 11.0f;

        // Start is called before the first frame update
        void OnEnable()
        {
            float vx = Random.Range(-vxMax, vxMax);
            float vy = Random.Range(vyMin, vyMax);
            v = new Vector2(vx, vy);
            gravity = Random.Range(gravityMin, gravityMax);

            float rad = Mathf.Atan2(vx, vy);
            rad *= Mathf.Rad2Deg;

            transform.localEulerAngles = new Vector3(0f, 0f, -rad + 180f);

            //Destroy(gameObject, lifeTime);
        }

        // Update is called once per frame
        void Update()
        {
            v.y += gravity * Time.deltaTime;
            transform.position += v * Time.deltaTime;

            if (IsOutSide()) gameObject.SetActive(false);
        }

        private bool IsOutSide()
        {
            if (transform.position.x < -borderX || transform.position.y > borderX || transform.position.y < -borderY)
                return true;
            return false;
        }

        /*private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "PlayerAttack")
            {
                Destroy(gameObject);
            }
        }*/
    }
}