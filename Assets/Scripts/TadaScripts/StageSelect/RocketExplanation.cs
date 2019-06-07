using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageSelect;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace StageSelect
{
    public class RocketExplanation : MonoBehaviour
    {
        [SerializeField]
        private Image backBlack;

        private void OnEnable()
        {
            StartCoroutine(BeginAnimation());
        }

        private IEnumerator BeginAnimation()
        {
            ChangeColor(GetComponent<TextMeshProUGUI>(), 1.0f, 0.5f);
            ChangeColor(backBlack, 0.5f, 0.5f);

            transform.DOLocalMoveY(
                0.0f, 0.5f);

            yield return new WaitForSeconds(2.0f);

            ChangeColor(GetComponent<TextMeshProUGUI>(), 0.0f, 0.3f);
            ChangeColor(backBlack, 0.0f, 0.3f);

            transform.DOLocalMoveY(
                -100f, 0.3f);

            yield return new WaitForSeconds(0.35f);

            gameObject.SetActive(false);
        }

        private void ChangeColor(TextMeshProUGUI text, float endValue, float duration)
        {
            DOTween.To(
               () => text.color,
                num => text.color = num,
                new Color(1f, 1f, 1f, endValue),
                duration);
        }
        private void ChangeColor(Image image, float endValue, float duration)
        {
            DOTween.To(
               () => image.color,
                num => image.color = num,
                new Color(0f, 0f, 0f, endValue),
                duration);
        }
    }
}