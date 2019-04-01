using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StageStateManager : MonoBehaviour
{
    //public static StageStateManager instance;

    public static int stageId = 0;
    private StageState nowStageState;
    public StageState firstStageState;
    [SerializeField]
    private TextMeshProUGUI stageNameText;
    [SerializeField]
    private Image stageImage;
    [SerializeField]
    private Image stageImageFlame;

    public Vector2 homing = new Vector2(0.2f,0.2f);
    public float stageAnimSpeed = 1.0f;
    private float pX, pY;
    private float targetX, targetY;

    // Start is called before the first frame update
    void Start()
    {
        //if (instance == null) instance = this;
        //else Destroy(gameObject);

        nowStageState = firstStageState;
        ChangeStageInfo();
        //Debug.Log("Set first Stage Stage");

        transform.position = nowStageState.stageTransform.position;

        //DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(nowStageState.stageName);
        pX = transform.position.x;
        pY = transform.position.y;
        pX = Mathf.Lerp(transform.position.x, targetX, homing.x);
        pY = Mathf.Lerp(transform.position.y, targetY, homing.y);

        transform.position = new Vector3(pX, pY, 0f);


        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (IsStageSpriteEnabled())
            {
                GoNextStage();
                return;
            }
            DisplayStageSprite();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            BlindStageSprite();
        }

        if (IsStageSpriteEnabled()) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!nowStageState.goRightStage(ref nowStageState)) return;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!nowStageState.goLeftStage(ref nowStageState)) return;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!nowStageState.goDownStage(ref nowStageState)) return;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!nowStageState.goUpStage(ref nowStageState)) return;
        }
        else return;

        ChangeStageInfo();
    }

    private void ChangeStageInfo()
    {
        stageImage.sprite = nowStageState.stageSprite;
        stageNameText.text = nowStageState.stageName;

        targetX = nowStageState.stageTransform.position.x;
        targetY = nowStageState.stageTransform.position.y;
    }

    private void DisplayStageSprite()
    {
        stageImageFlame.gameObject.SetActive(true);
        stageImageFlame.rectTransform.localPosition = new Vector3(0f, -1000f, 0f); // 見えない位置に配置
        stageImageFlame.rectTransform.DOMove(
            stageImageFlame.rectTransform.position + new Vector3(0, 900f, 0),
            stageAnimSpeed
            )
            .SetEase(Ease.OutBack);
    }

    private void BlindStageSprite()
    {
        stageImageFlame.rectTransform.DOKill();
        stageImageFlame.gameObject.SetActive(false);
    }

    private bool IsStageSpriteEnabled()
    {
        return stageImageFlame.gameObject.activeSelf;
    }

    private void GoNextStage()
    {
        SceneManager.LoadScene(nowStageState.stageSceneName);
    }
}
