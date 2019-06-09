using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HukidashiController : MonoBehaviour
{
    public float lifeTime = 1.0f;
    public bool isExist = false;
    public float displayTime = 0.5f;


    public void FlowStart()
    {
        StartCoroutine(Flow());
    }

    private IEnumerator Flow()
    {
        AppearIn(1.66f, 0.5f);

        yield return new WaitForSeconds(displayTime);

        AppearOut(0f, 0.5f);
    }

    private void AppearIn(float endValue, float duration)
    {
       //Debug.Log("aaaa");
        isExist = true;
        transform.DOScaleY(
            endValue,
            duration).SetEase(Ease.OutBack);
    }

    private void AppearOut(float endValue, float duration)
    {
        transform.DOScaleY(
            endValue,
            duration).SetEase(Ease.InBack).OnComplete(() => isExist = false);
    }
}
