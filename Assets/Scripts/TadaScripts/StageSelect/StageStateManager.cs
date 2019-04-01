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
    public static int nowStageId = 0;
    private StageState nowStageState;
    [SerializeField]
    private TextMeshProUGUI stageNameText;
    [SerializeField]
    private Image stageImage;
    [SerializeField]
    private Image stageImageFlame;
    [SerializeField]
    private GameObject stageObjectList;

    public Vector2 moveHoming = new Vector2(0.2f,0.2f);
    public float stageAnimSpeed = 1.0f;
    private float playerX, playerY;
    private float targetX, targetY;

    // Start is called before the first frame update
    void Start()
    {
        //if (instance == null) instance = this;
        //else Destroy(gameObject);

        // すべてのstageStateから現在のステージ場所を全探索する テーブル作るより楽で簡潔になった
        foreach (Transform child in stageObjectList.transform) // 少し重そうなのが欠点
        {
            if (nowStageId == child.GetComponent<StageState>().stageId)
            {
                nowStageState = child.GetComponent<StageState>();
                break;
            }
        }

        ChangeStageInfo();

        transform.position = nowStageState.stageTransform.position;
        // ついでにカメラの位置も変える
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);

        //DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(nowStageState.stageName);
        playerX = transform.position.x;
        playerY = transform.position.y;
        playerX = Mathf.Lerp(transform.position.x, targetX, moveHoming.x);
        playerY = Mathf.Lerp(transform.position.y, targetY, moveHoming.y);

        transform.position = new Vector3(playerX, playerY, 0f);


        if (IsStageSpriteEnabled())
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GoNextStage();
                return;
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                BlindStageSprite();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
            DisplayStageSprite();

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

    // ステージのステート変更に伴う変更
    private void ChangeStageInfo()
    {
        nowStageId = nowStageState.stageId;

        stageImage.sprite = nowStageState.stageSprite;
        stageNameText.text = nowStageState.stageName;

        targetX = nowStageState.stageTransform.position.x;
        targetY = nowStageState.stageTransform.position.y;
    }

    // ステージの画像を表示する
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

    // ステージの画像を消す
    private void BlindStageSprite()
    {
        stageImageFlame.rectTransform.DOKill();
        stageImageFlame.gameObject.SetActive(false);
    }

    // ステージの画像が表示されているか
    private bool IsStageSpriteEnabled()
    {
        return stageImageFlame.gameObject.activeSelf;
    }

    // 目的のステージへ遷移する
    private void GoNextStage()
    {
        SceneManager.LoadScene(nowStageState.stageSceneName);
    }
}
