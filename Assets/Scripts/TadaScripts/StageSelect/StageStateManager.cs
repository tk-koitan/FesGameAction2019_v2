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

    [System.Serializable]
    public class ArrowList
    {
        public GameObject upArrow;
        public GameObject rightArrow;
        public GameObject downArrow;
        public GameObject leftArrow;
    }
    public ArrowList arrowList;
    [SerializeField]
    private GameObject arrow;

    public Vector2 moveHoming = new Vector2(0.2f,0.2f);
    public float stageAnimSpeed = 1.0f;
    private float playerX, playerY;
    private float targetX, targetY;

    public float speed = 15.0f;

    private int passedNum = 0;
    private bool isMoving = false;
    private bool prevIsMoving = false;

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
        SetArrow();
        ShowStageName();

        transform.position = nowStageState.transform.position;
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

        if (isMoving != prevIsMoving)
        {
            SwitchArrow();
            if(!isMoving)
                ShowStageName();
        }

        prevPos = transform.position;
        prevIsMoving = isMoving;


        if(dir == 1f)
            arrow.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        else
            arrow.transform.localEulerAngles = new Vector3(0f, 180f, 0f);

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

        if (ActionInput.GetButtonDown(ButtonCode.UpArrow))
        {
            if (!nowStageState.canMove.up) return;
            if (nowStageState.nextStage.upStage == null) return;
            
            moveDir = MoveDirection.UP;
        }
        else if (ActionInput.GetButtonDown(ButtonCode.RightArrow))
        {
            if (!nowStageState.canMove.right) return;
            if (nowStageState.nextStage.rightStage == null) return;

            moveDir = MoveDirection.RIGHT;
        }
        else if (ActionInput.GetButtonDown(ButtonCode.DownArrow))
        {
            if (!nowStageState.canMove.down) return;
            if (nowStageState.nextStage.downStage == null) return;

            moveDir = MoveDirection.DOWN;
        }
        else if (ActionInput.GetButtonDown(ButtonCode.LeftArrow))
        {
            if (!nowStageState.canMove.left) return;
            if (nowStageState.nextStage.leftStage == null) return;

            moveDir = MoveDirection.LEFT;
        }
        else return;

        MoveAction(moveDir);
        ChangeStageState(moveDir);
    }

    // 道のりに沿って移動する
    private void MoveAction(MoveDirection dir)
    {
        isMoving = true;

        switch (dir)
        {
            case MoveDirection.UP:
                MoveAppend(nowStageState.stagePath.upPath, dir);
                break;
            case MoveDirection.RIGHT:
                MoveAppend(nowStageState.stagePath.rightPath, dir);
                break;
            case MoveDirection.DOWN:
                MoveAppend(nowStageState.stagePath.downPath, dir);
                break;
            case MoveDirection.LEFT:
                MoveAppend(nowStageState.stagePath.leftPath, dir);
                break;
        }
    }
    
    private void MoveAppend(Transform[] pathTransform, MoveDirection dir)
    {
        //Sequence moveSequence = DOTween.Sequence();
        Vector3[] path = new Vector3[pathTransform.Length + 1];
        float time;
        for (int i = 0; i < pathTransform.Length; i++)
        {
            path[i] = pathTransform[i].position;
            /*
            time = Vector2.Distance(path[0], path[1]) / speed;
            moveSequence.Append(
                transform.DOLocalPath(
                    path,
                    time,
                    PathType.CatmullRom).SetEase(Ease.Linear));
            path[0] = path[1];*/
        }

        switch (dir)
        {
            case MoveDirection.UP:
                path[pathTransform.Length] = nowStageState.nextStage.upStage.transform.position;
                time = Vector2.Distance(transform.position, 
                    nowStageState.nextStage.upStage.transform.position) / speed;
                break;
            case MoveDirection.RIGHT:
                path[pathTransform.Length] = nowStageState.nextStage.rightStage.transform.position;
                time = Vector2.Distance(transform.position, 
                    nowStageState.nextStage.rightStage.transform.position) / speed;
                break;
            case MoveDirection.DOWN:
                path[pathTransform.Length] = nowStageState.nextStage.downStage.transform.position;
                time = Vector2.Distance(transform.position, 
                    nowStageState.nextStage.downStage.transform.position) / speed;
                break;
            case MoveDirection.LEFT:
                path[pathTransform.Length] = nowStageState.nextStage.leftStage.transform.position;
                time = Vector2.Distance(transform.position, 
                    nowStageState.nextStage.leftStage.transform.position) / speed;
                break;
            default: // null以外にしたいため意味なし
                path[pathTransform.Length] = transform.position;
                time = 0f;
                Debug.Log("Stage Path Error");
                break;
        }
        /*
        time = Vector2.Distance(path[0], path[1]) / speed;
        moveSequence.Append(
            moveSequence.Append(
                transform.DOLocalPath(
                    path,
                    time,
                    PathType.CatmullRom).SetEase(Ease.Linear)).OnComplete(() => isMoving = false));
                    */
        //moveSequence.Append(
           // moveSequence.Append(
            transform.DOLocalPath(
               path,
               time,
               PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(() => isMoving = false);
    }

    // ステージステートを移動先のにセットする
    private void ChangeStageState(MoveDirection dir)
    {
        switch (dir)
        {
            case MoveDirection.UP:
                nowStageState = nowStageState.nextStage.upStage;
                break;
            case MoveDirection.RIGHT:
                nowStageState = nowStageState.nextStage.rightStage;
                break;
            case MoveDirection.DOWN:
                nowStageState = nowStageState.nextStage.downStage;
                break;
            case MoveDirection.LEFT:
                nowStageState = nowStageState.nextStage.leftStage;
                break;
        }
        ChangeStageInfo();
        SwitchArrow();
    }

    // ステージのステート変更に伴う変更
    private void ChangeStageInfo()
    {
        nowStageId = nowStageState.stageId;

        stageImage.sprite = nowStageState.stageSprite;
        stageNameText.text = "";
        //stageNameText.text = nowStageState.stageName;

        SetArrow();
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

    // 左右上下に移動できるかの矢印
    private void SetArrow()
    {
        arrowList.upArrow.SetActive(nowStageState.canMove.up);
        arrowList.rightArrow.SetActive(nowStageState.canMove.right);
        arrowList.downArrow.SetActive(nowStageState.canMove.down);
        arrowList.leftArrow.SetActive(nowStageState.canMove.left);
    }

    // 移動中は矢印を消す
    private void SwitchArrow()
    {
        arrow.SetActive(!isMoving);
    }

    // ステージ名を表示する
    private void ShowStageName()
    {
        stageNameText.text = nowStageState.stageName;
    }

    // 目的のステージへ遷移する
    private void GoNextStage()
    {
        // 挑戦するステージのidを保存しておく
        StageSelect.StageTable.challengeStageId = nowStageId;
        SceneManager.LoadScene(nowStageState.stageSceneName);
    }
}
