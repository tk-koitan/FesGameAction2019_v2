using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StageStateManager : MonoBehaviour
{
    public static StageState nowStageState;
    public StageState firstStageState;
    [SerializeField]
    private TextMeshProUGUI stageNameText;

    public Vector2 homing = new Vector2(0.2f,0.2f);
    private float pX, pY;

    // Start is called before the first frame update
    void Start()
    {
        if (nowStageState == null) nowStageState = firstStageState;
        transform.position = nowStageState.stageTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pX = transform.position.x;
        pY = transform.position.y;
        pX = Mathf.Lerp(transform.position.x, nowStageState.stageTransform.position.x, homing.x);
        pY = Mathf.Lerp(transform.position.y, nowStageState.stageTransform.position.y, homing.y);

        transform.position = new Vector3(pX, pY, 0f);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            DisplayStageSprite();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            BlindStageSprite();
        }

        if (IsStageSpriteEnabled()) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(nowStageState.goRightStage(ref nowStageState))
            {
                ChangeStageSprite();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(nowStageState.goLeftStage(ref nowStageState))
            {
                ChangeStageSprite();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(nowStageState.goDownStage(ref nowStageState))
            {
                ChangeStageSprite();
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(nowStageState.goUpStage(ref nowStageState))
            {
                ChangeStageSprite();
            }
        }
    }

    private void ChangeStageSprite()
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().sprite = nowStageState.stageSprite;
        }
        stageNameText.text = nowStageState.stageName;
    }

    private void DisplayStageSprite()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void BlindStageSprite()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private bool IsStageSpriteEnabled()
    {
        foreach (Transform child in transform)
        {
            return child.gameObject.activeSelf;
        }
        return false;
    }
}
