using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

enum MoveDirection
{
    RIGHT,
    LEFT,
    UP,
    DOWN,
}

public class StageStateManager : MonoBehaviour
{
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
    [SerializeField]
    private GameObject[] arrowObjectList;
    [SerializeField]
    private GameObject arrow;

    public Vector2 moveHoming = new Vector2(0.2f,0.2f);
    public float stageAnimSpeed = 1.0f;
    private float playerX, playerY;
    private float targetX, targetY;

    public float speed = 15.0f;

    private int passedNum = 0;
    private bool isMoving = false;

    Animator animator;

    private Vector2 prevPos;
    private float dir;
    private float defaultScaleX;

    // Start is called before the first frame update
    void Start()
    {
        dir = (transform.localScale.x > 0.0f) ? 1 : -1; // tada
        defaultScaleX = transform.localScale.x * dir;
        transform.localScale = new Vector3(
            defaultScaleX, transform.localScale.y, transform.localScale.z);

        animator = GetComponent<Animator>();

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
        ShowArrow();

        transform.position = nowStageState.stageTransform.position;
        // ついでにカメラの位置も変える
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);

        prevPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(isMoving);
        if (transform.position.x - prevPos.x > 0) dir = 1f;
        else if(transform.position.x - prevPos.x < 0) dir = -1f;
        animator.SetFloat("MoveSpeed", (isMoving) ? 10 : 0);
        transform.localScale = new Vector3(
            defaultScaleX * dir, transform.localScale.y, transform.localScale.z);

        prevPos = transform.position;

        SwitchArrow();

        if (isMoving) return;

        if (IsStageSpriteEnabled())
        {
            if (ActionInput.GetButtonDown(ButtonCode.Jump))
            {
                GoNextStage();
                return;
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                BlindStageSprite();
            }
        }

        if (ActionInput.GetButtonDown(ButtonCode.Jump))
            DisplayStageSprite();
        

        if (IsStageSpriteEnabled()) return;

        MoveDirection moveDir;

        if (ActionInput.GetButtonDown(ButtonCode.RightArrow))
        {
            if (!nowStageState.goRightStage(ref nowStageState)) return;
            moveDir = MoveDirection.RIGHT;
        }
        else if (ActionInput.GetButtonDown(ButtonCode.LeftArrow))
        {
            if (!nowStageState.goLeftStage(ref nowStageState)) return;
            moveDir = MoveDirection.LEFT;
        }
        else if (ActionInput.GetButtonDown(ButtonCode.DownArrow))
        {
            if (!nowStageState.goDownStage(ref nowStageState)) return;
            moveDir = MoveDirection.DOWN;
        }
        else if (ActionInput.GetButtonDown(ButtonCode.UpArrow))
        {
            if (!nowStageState.goUpStage(ref nowStageState)) return;
            moveDir = MoveDirection.UP;
        }
        else return;

        MoveAction(moveDir);
        ChangeStageInfo();
        ShowArrow();
    }

    // 道のりに沿って移動する
    private void MoveAction(MoveDirection dir)
    {
        isMoving = true;

        switch (dir)
        {
            case MoveDirection.RIGHT:
                MoveAppend(nowStageState.fromLeftPath);
                break;
            case MoveDirection.LEFT:
                MoveAppend(nowStageState.fromRightPath);
                break;
            case MoveDirection.UP:
                MoveAppend(nowStageState.fromDownPath);
                break;
            case MoveDirection.DOWN:
                MoveAppend(nowStageState.fromUpPath);
                break;
        }
    }
    
    private void MoveAppend(Transform[] pathTransform)
    {
        Sequence moveSequence = DOTween.Sequence();
        Vector3 from, to;
        float time;
        from = transform.position;
        for (int i = 0; i < pathTransform.Length; i++)
        {
            to = pathTransform[i].position;
            time = Vector2.Distance(from, to) / speed;
            moveSequence.Append(
                transform.DOMove(
                    to,
                    time));
            from = to;
        }
        to = nowStageState.stageTransform.position;
        time = Vector2.Distance(from, to) / speed;
        moveSequence.Append(
            transform.DOMove(
                to,
                time).OnComplete(() => isMoving = false));
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
        stageImageFlame.rectTransform.DOLocalMoveY(
            -100f,
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

    // 矢印を表示する
    private void ShowArrow()
    {
        bool[] arrowExist = nowStageState.GetStageExist();
        for(int i = 0; i < 4; i++)
        {
            arrowObjectList[i].SetActive(arrowExist[i]);
        }
    }

    // 移動中は矢印を消す あとでちゃんと書く
    private void SwitchArrow()
    {
        arrow.SetActive(!isMoving);
    }

    // 目的のステージへ遷移する
    private void GoNextStage()
    {
        // 挑戦するステージのidを保存しておく
        StageSelect.StageTable.challengeStageId = nowStageId;
        SceneManager.LoadScene(nowStageState.stageSceneName);
    }
}
