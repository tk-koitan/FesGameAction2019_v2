using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using StageSelect;
using KoitanLib;

namespace StageSelect
{
    public class RocketFlagmentController : MonoBehaviour
    {
        [System.Serializable]
        public class RocketFlagment
        {
            public SpriteRenderer rocketSprite;
            public Vector3[] pathPosition;
        }

        public RocketFlagment[] rocketFlagment;

        // Start is called before the first frame update
        void Start()
        {
            foreach (RocketFlagment child in rocketFlagment)
            {
                if (child.pathPosition.Length <= 1) continue;
                child.rocketSprite.transform.DOLocalPath(
                    child.pathPosition,
                    10.0f,
                    PathType.CatmullRom);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach(RocketFlagment child in rocketFlagment)
            {
                Vector3 from = child.rocketSprite.transform.position;
                Vector3 to;
                foreach(Vector3 path in child.pathPosition)
                {
                    to = path;
                    GizmosExtensions2D.DrawArrow2D(from, to);
                    from = to;
                }
            }
        }
#endif
    }
}