using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public TextMeshProUGUI titleUi;
    public Image cursor;
    public float width;
    private int index = 0;
    private int maxIndex;
    private Vector3 cursorDefaultPos;
    // Start is called before the first frame update
    void Start()
    {
        cursorDefaultPos = cursor.transform.localPosition;
        maxIndex = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (ActionInput.GetButtonDown(ButtonCode.UpArrow))
        {
            index--;
            index = (index + maxIndex) % maxIndex;
        }

        if (ActionInput.GetButtonDown(ButtonCode.DownArrow))
        {
            index++;
            index %= maxIndex;
        }

        cursor.transform.localPosition = cursorDefaultPos + Vector3.down * width * index;
    }
}
