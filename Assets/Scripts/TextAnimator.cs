using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAnimator : MonoBehaviour
{
    public TextMeshProUGUI tmproText;

    public void TextAnim(string text)
    {
        tmproText.text = text;
    }
}
