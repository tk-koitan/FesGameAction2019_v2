using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using StageSelect;

namespace StageSelect
{
    public class StageChangeFrogUI : MonoBehaviour
    {
        [SerializeField]
        RectTransform rectTran;

        public Vector3 beforePos = new Vector3(100f, -100f, 0f);
        public float stampTime = 1.0f;
        public Vector3 endScale = new Vector3(3f, 3f, 1f);

        // Start is called before the first frame update
        void Start()
        {
            rectTran.localPosition -= beforePos;
            StartCoroutine(StampAnim());
        }

        private IEnumerator StampAnim()
        {
            rectTran.DOLocalMove(
                rectTran.localPosition + beforePos,
                stampTime
                );

            rectTran.DOScale(
                endScale,
                stampTime);

            yield return new WaitForSeconds(stampTime + 1.0f);

            DOTween.To(
                () => GetComponent<Image>().color,
                num => GetComponent<Image>().color = num,
                new Color(1f, 1f, 1f, 1f),
                1.0f
                );
            /*
            rectTran.DOScale(
                endScale * 0.2f,
                1.5f
                );
            rectTran.DOLocalRotate(
                new Vector3(0f, 0f, -180f),
                2.5f
                );
                */
        }
    }
}