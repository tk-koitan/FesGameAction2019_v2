using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DropRocketScene;

namespace DropRocketScene
{
    public class DropRocket : MonoBehaviour
    {
        public Vector3 destinationPos;

        public float arriveTime = 3.0f;

        [SerializeField]
        private ParticleSystem explosioinEffect;

        /*[System.Serializable]
        public class RocketFlagment
        {
            public SpriteRenderer rocketSprite;
            public Transform destination;
            public float upPower = 5.0f;
        }*/

        public SpriteRenderer[] rocketFlagment;
        public SpriteRenderer[] otamatan;

        public PlayerDrop player;

        [SerializeField]
        private SpriteRenderer rocketArea;


        //public RocketFlagment[] rocketFlagment;

        // Start is called before the first frame update
        void Start()
        {
            transform.DOMove(
                destinationPos,
                arriveTime).SetEase(Ease.InSine).OnComplete(() => Explosion());
        }

        private void Explosion()
        {
            explosioinEffect.gameObject.SetActive(true);
            foreach(SpriteRenderer flagment in rocketFlagment)
            {
                flagment.gameObject.SetActive(true);
            }
            foreach (SpriteRenderer child in otamatan)
            {
                child.gameObject.SetActive(true);
            }
            player.gameObject.SetActive(true);
            rocketArea.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}