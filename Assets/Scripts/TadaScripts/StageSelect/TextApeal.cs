using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextApeal : MonoBehaviour
{
    private Vector2 defaultScale;
    // Start is called before the first frame update
    void Start()
    {
        defaultScale = transform.localScale;
        transform
            .DOScale(defaultScale.x * 1.3f, defaultScale.y * 1.3f)
            .SetLoops(-1, LoopType.Yoyo)
            .Play();
    }
}
