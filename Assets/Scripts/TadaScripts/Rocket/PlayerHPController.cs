using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;
using DG.Tweening;
using UnityEngine.UI;

namespace RocketStage
{
    public class PlayerHPController : MonoBehaviour
    {
        [SerializeField]
        private RectTransform setPosition;
        [SerializeField]
        private float time = 2.0f;
        [SerializeField]
        private float breakCircle = 10.0f;

        //[SerializeField]
        //private float speed = 50.0f;

        private void OnEnable()
        {
            //float distance = Vector2.Distance(transform.position, setPosition.position);
            //float time = distance / speed;
            //Debug.Log("この座標にいきます : " + setPosition.position);
            transform.DOMove(
                setPosition.position,
                time
                ).SetEase(Ease.OutSine);
        }

        public void DestroyCoin()
        {
            // コインを3つに分解し、フェード消滅
            foreach(Transform child in transform)
                StartCoroutine(BreakCoin(child.GetComponent<Image>()));
        }

        private IEnumerator BreakCoin(Image coinFlagment)
        {
            float difX = Random.Range(-breakCircle, breakCircle);
            float difY = Random.Range(-breakCircle, breakCircle);

            coinFlagment.rectTransform.DOMove(
                new Vector3(coinFlagment.rectTransform.position.x + difX, coinFlagment.rectTransform.position.y + difY, 0f),
                time).SetEase(Ease.OutQuart);

            yield return new WaitForSeconds(time / 2f);

            DOTween.To(
                () => coinFlagment.color,
                num => coinFlagment.color = num,
                new Color(1f, 1f, 1f, 0f),
                time / 2f);

            yield return new WaitForSeconds(time / 2f);

            Destroy(coinFlagment.gameObject);
        }
        /*
        private void OnValidate()
        {
            setPosition = transform.localPosition;
        }*/
    }
}