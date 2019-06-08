using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StaffRoll;
using DG.Tweening;
using TMPro;

namespace StaffRoll
{
    public class StaffRollText : MonoBehaviour
    {
        public float endSize = 1.0f;
        public float duration = 2.0f;
        public float lifeTime = 5.0f;

        private bool isTrigger = true;

        TextMeshProUGUI tmpro;

        private void Awake()
        {
            tmpro = GetComponent<TextMeshProUGUI>();
        }

        // Start is called before the first frame update
        void OnEnable()
        {
            ScaleUp(duration);
            //Feed(1.0f, duration/2f);
            Invoke("Destroy", lifeTime);
        }

        private void Update()
        {
            if (ActionInput.GetButtonDown(ButtonCode.Jump))
            {
                if (isTrigger) hoge();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isTrigger) return;

            // 色を変えて回転させる
            tmpro.color =
                new Color(1f, 1f, 0f, tmpro.color.a);

            tmpro.rectTransform.DORotate(new Vector3(0, -180, 0), 1).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

            isTrigger = false;
        }

        void hoge()
        {
            // 色を変えて回転させる
            tmpro.color =
                new Color(1f, 1f, 0f, tmpro.color.a);

            tmpro.rectTransform.DORotate(new Vector3(0, -180, 0), 1).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            isTrigger = false;

        }

        // 大きさ拡大
        private void ScaleUp(float _duration)
        {
            tmpro.rectTransform.DOScale(
                endSize,
                _duration).SetEase(Ease.OutQuad);
        }

        // α値を変更
        private void Feed(float endValue, float _duration)
        {
            DOTween.To(
                () => tmpro.color,
                num => tmpro.color = num,
                new Color(tmpro.color.r,
                tmpro.color.g, tmpro.color.b, endValue),
                _duration
                ).OnComplete(() => isTrigger = true);
        }

        private void Destroy()
        {
            Feed(0.0f, duration / 2f);
        }
    }
}