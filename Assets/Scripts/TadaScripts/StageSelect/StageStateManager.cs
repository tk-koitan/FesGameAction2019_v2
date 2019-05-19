using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Cinemachine;
using StageSelect;

namespace StageSelect
{
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
        private CinemachineVirtualCamera vcamera;
        [SerializeField]
        private GameObject flogUI;

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

        public Vector2 moveHoming = new Vector2(0.2f, 0.2f);
        public float stageAnimSpeed = 1.0f;

        public float speed = 15.0f;
        public float sceneWaitTime = 3.0f;

        private bool isMoving = false;
        private bool prevIsMoving = false;
        private bool actionEnabled = true;

        Animator animator;

        private Vector2 prevPos;
        private float dir;
        private float defaultScaleX;

        MenuController menuCtrl;

        // Start is called before the first frame update
        void Start()
        {
            dir = (transform.localScale.x > 0.0f) ? 1 : -1; // tada
            defaultScaleX = transform.localScale.x * dir;
            transform.localScale = new Vector3(
                defaultScaleX, transform.localScale.y, transform.localScale.z);

            animator = GetComponent<Animator>();
            menuCtrl = GetComponent<MenuController>();

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
            //Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);

            prevPos = transform.position;

            MusicManager.Play(MusicManager.Instance.bgm2);
        }

        // Update is called once per frame
        void Update()
        {
            // Debug.Log(isMoving);
            // 後でプレイヤー部分とステージステート部分を分けたい

            if (!actionEnabled) return;

            if (transform.position.x - prevPos.x > 0) dir = 1f;
            else if (transform.position.x - prevPos.x < 0) dir = -1f;
            animator.SetFloat("MoveSpeed", (isMoving) ? 10 : 0);
            transform.localScale = new Vector3(
                defaultScaleX * dir, transform.localScale.y, transform.localScale.z);

            if (isMoving != prevIsMoving)
            {
                SwitchArrow(!isMoving);
                if (!isMoving)
                    ShowStageName();
            }

            prevPos = transform.position;
            prevIsMoving = isMoving;


            if (dir == 1f)
                arrow.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            else
                arrow.transform.localEulerAngles = new Vector3(0f, 180f, 0f);

            if (isMoving) return;

            // あまりよくない
            if(!ActionInput.actionEnabled)
                SwitchArrow(ActionInput.actionEnabled);

            if (IsStageSpriteEnabled())
            {
                if (ActionInput.GetButtonDown(ButtonCode.Jump))
                {
                    StartCoroutine(GoNextStage(sceneWaitTime));
                    return;
                }
                else if (ActionInput.GetButtonDown(ButtonCode.Cancel))
                {
                    BlindStageSprite();
                    SwitchArrow(true);
                }
            }

            if (ActionInput.GetButtonDown(ButtonCode.Jump))
            {
                if (!menuCtrl.isDisplayed)
                {
                    DisplayStageSprite();
                    SwitchArrow(false);
                }
            }


            if (IsStageSpriteEnabled()) return;

            if (menuCtrl != null)
            {
                if (menuCtrl.isDisplayed)
                {/*
                if (ActionInput.GetButtonDown(ButtonCode.Cancel))
                {
                    menuCtrl.EndMenu();
                }*/
                    return;
                }

                if (Input.GetKeyDown(KeyCode.P))
                    menuCtrl.StartMenu();
            }

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
                    MoveAppend(nowStageState.stagePath.upPath,
                        nowStageState.nextStage.upStage.transform.position);
                    break;
                case MoveDirection.RIGHT:
                    MoveAppend(nowStageState.stagePath.rightPath,
                        nowStageState.nextStage.rightStage.transform.position);
                    break;
                case MoveDirection.DOWN:
                    MoveAppend(nowStageState.stagePath.downPath,
                        nowStageState.nextStage.downStage.transform.position);
                    break;
                case MoveDirection.LEFT:
                    MoveAppend(nowStageState.stagePath.leftPath,
                        nowStageState.nextStage.leftStage.transform.position);
                    break;
            }
        }

        // DOTweenで移動する
        private void MoveAppend(Transform[] pathTransform, Vector3 destination)
        {
            Vector3[] path = new Vector3[pathTransform.Length + 1];
            float time;
            for (int i = 0; i < pathTransform.Length; i++)
            {
                path[i] = pathTransform[i].position;
            }

            path[pathTransform.Length] = destination;

            time = Vector2.Distance(transform.position, destination) / speed; ;

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
            SwitchArrow(!isMoving);
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
            vcamera.gameObject.SetActive(false);
            stageImageFlame.gameObject.SetActive(true);
            stageImageFlame.rectTransform.localPosition = new Vector3(-1600f, -100f, 0f); // 見えない位置に配置
            stageImageFlame.rectTransform.DOLocalMoveX(
                -400f,
                stageAnimSpeed
                )
                .SetEase(Ease.OutBack);
        }

        // ステージの画像を消す
        private void BlindStageSprite()
        {
            vcamera.gameObject.SetActive(true);
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
        private void SwitchArrow(bool isActive)
        {
            if(arrow.activeSelf != isActive)
                arrow.SetActive(isActive);
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

        // 目的のステージへ遷移する
        private IEnumerator GoNextStage(float time)
        {
            // 挑戦するステージのidを保存しておく
            StageSelect.StageTable.challengeStageId = nowStageId;
            actionEnabled = false;
            animator.SetTrigger("Clear");

            yield return new WaitForSeconds(time);

            foreach (Transform flogChild in flogUI.transform)
            {
                yield return new WaitForSeconds(0.01f);
                flogChild.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(2.2f);

            SceneManager.LoadScene(nowStageState.stageSceneName);
        }
    }
}