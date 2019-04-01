using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextApeal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform
            .DOScale(1.3f, 1.3f)
            .SetLoops(-1, LoopType.Yoyo)
            .Play();
    }
}
