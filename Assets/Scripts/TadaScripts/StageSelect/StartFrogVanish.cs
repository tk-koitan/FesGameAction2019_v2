using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartFrogVanish : MonoBehaviour
{
    [SerializeField]
    GameObject flogUI;

    public float animTime = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FlogVanish());
    }

    private IEnumerator FlogVanish()
    {
        foreach(Transform childFlog in flogUI.transform)
        {
            childFlog.DOScale(
                Vector3.zero,
                1.5f
                );
            childFlog.DOLocalRotate(
                new Vector3(0f, 0f, -180f),
                1.5f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
