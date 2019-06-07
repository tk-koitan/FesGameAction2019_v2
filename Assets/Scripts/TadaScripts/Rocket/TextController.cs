using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using RocketStage;

namespace RocketStage
{
    public class TextController : MonoBehaviour
    {

        [System.Serializable]
        public class Conversation
        {
            public string scenario;
            public int charaIndex;
        }

        public Conversation[] scenarios;
        [SerializeField]
        private TextMeshProUGUI uiText;

        [SerializeField]
        [Range(0.001f, 0.3f)]
        private float intervalForCharacterDisplay = 0.05f; // 1文字の表示にかかる時間

        [SerializeField]
        private Image conversationField;
        [SerializeField]
        private TextMeshProUGUI warningText;
        [SerializeField]
        private Image leftChara;
        [SerializeField]
        private Image rightChara;
        [SerializeField]
        private Image lastBossChara;

        private int currentLine = 0; // 現在の行番号
        private string currentText = string.Empty;
        private float timeUntilDisplay = 0;
        private float timeElapsed = 1;
        private int lastUpdateCharacter = -1;

        private bool isEventing = false;
        private bool isPrepareTime = true;

        public bool isConversationing { private set; get; }

        // 文字の表示が完了しているかどうか
        public bool IsCompleteDisplayText
        {
            get { return Time.time > timeElapsed + timeUntilDisplay; }
        }

        //by koitan
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip messageSE;
        [SerializeField]
        private AudioClip warningSE;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void ScenarioStart()
        {
            isPrepareTime = true;
            isConversationing = true;
            conversationField.rectTransform.DOScaleX(
                1.0f,
                1.0f).SetEase(Ease.OutCubic)
                .OnComplete(() => StartText());
            uiText.text = "";
            leftChara.gameObject.SetActive(false);
            rightChara.gameObject.SetActive(false);
            lastBossChara.gameObject.SetActive(false);
        }

        private void StartText()
        {
            isPrepareTime = false;
            SetNextLine();
        }

        public void ScenerioEnd()
        {
            isPrepareTime = true;
            isConversationing = false;
            conversationField.rectTransform.DOScaleX(
                0f, 1.0f).SetEase(Ease.InCubic);
            uiText.text = "";
            leftChara.gameObject.SetActive(false);
            rightChara.gameObject.SetActive(false);
            lastBossChara.gameObject.SetActive(false);
        }

        public void ScenarioUpdate()
        {
            if (isPrepareTime) return;

            /*if (ActionInput.GetButtonDown(ButtonCode.Cancel))
            {
                ScenerioEnd();
            }*/

            if (isEventing)
            {
                // Warningのアニメーションを表示
                if (!warningText.gameObject.activeSelf)
                {
                    warningText.gameObject.SetActive(true);
                    StartCoroutine(WarningAnimation());
                }
                return;
            }

            // 文字表示が完了しているなら、決定キー後に次の行を表示
            if (ActionInput.GetButtonDown(ButtonCode.Jump))
            {
                if (IsCompleteDisplayText)
                {

                    if (currentLine < scenarios.Length)
                        SetNextLine();
                    else
                        ScenerioEnd();
                }
                // 完了していないなら文字をすべて表示する
                else
                    timeUntilDisplay = 0;
            }


            int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) /
                timeUntilDisplay) * currentText.Length);

            if (displayCharacterCount != lastUpdateCharacter)
            {
                uiText.text = currentText.Substring(0, displayCharacterCount);
                lastUpdateCharacter = displayCharacterCount;

                //by koitan
                audioSource.PlayOneShot(messageSE);
            }
        }

        void SetNextLine()
        {   
            currentText = scenarios[currentLine++].scenario;
            // イベント処理かどうか
            if (currentText == "event")
            {
                isEventing = true;
                leftChara.gameObject.SetActive(false);
                rightChara.gameObject.SetActive(false);
                lastBossChara.gameObject.SetActive(false);
            }

            // 想定表示時間と現在の時刻をキャッシュ
            timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
            timeElapsed = Time.time;

            // 文字カウントを初期化
            lastUpdateCharacter = -1;

            if (isEventing) return;

            if (scenarios[currentLine - 1].charaIndex == 0)
                uiText.color = new Color(0.2f, 0.6f, 0.0f, 1.0f);
            else
                uiText.color = new Color(0.2f, 0.2f, 0.2f, 1.0f);

            if (scenarios[currentLine - 1].charaIndex == 0)
            {
                leftChara.gameObject.SetActive(true);
                rightChara.gameObject.SetActive(false);
                lastBossChara.gameObject.SetActive(false);
                //by koitan
                audioSource.pitch = 1.5f;
            }
            else if(scenarios[currentLine - 1].charaIndex == 1)
            {
                leftChara.gameObject.SetActive(false);
                rightChara.gameObject.SetActive(true);
                lastBossChara.gameObject.SetActive(false);
                //by koitan
                audioSource.pitch = 3f;
            }
            else
            {
                leftChara.gameObject.SetActive(false);
                rightChara.gameObject.SetActive(false);
                lastBossChara.gameObject.SetActive(true);

                audioSource.pitch = 0.8f;
            }
        }

        private IEnumerator WarningAnimation()
        {
            MusicManager.Play(MusicManager.Instance.bgm5);

            audioSource.pitch = 1.0f;
            audioSource.PlayOneShot(warningSE);

            Vector3 defaultScale = warningText.rectTransform.localScale;
            Vector3 popScale = defaultScale * 2.0f;

            for(int i = 0; i < 4; i++)
            {
                warningText.rectTransform.DOScale(
                    popScale,
                    0.5f);

                yield return new WaitForSeconds(0.5f);

                warningText.rectTransform.DOScale(
                    defaultScale,
                    0.5f);

                yield return new WaitForSeconds(0.5f);
            }
            //warningText.rectTransform.DOPunchScale(
            //    transform.localScale * 1.5f, 4.0f).OnComplete(() => EndWarningEvent());
            //warningText.rectTransform.DOShakeScale(
            //4.0f).OnComplete(() => EndWarningEvent());
            EndWarningEvent();
        }

        private void EndWarningEvent()
        {
            isEventing = false;
            warningText.gameObject.SetActive(false);
            SetNextLine();
        }
    }
}