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

        public void ScenarioStart()
        {
            isPrepareTime = true;
            isConversationing = true;
            conversationField.rectTransform.DOScaleX(
                1.0f,
                1.0f).SetEase(Ease.OutCubic)
                .OnComplete(() => StartText());
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
        }

        public void ScenarioUpdate()
        {
            if (isPrepareTime) return;

            if (ActionInput.GetButtonDown(ButtonCode.Cancel))
            {
                ScenerioEnd();
            }

            if (isEventing)
            {
                // Warningのアニメーションを表示
                if (!warningText.gameObject.activeSelf)
                {
                    warningText.gameObject.SetActive(true);
                    WarningAnimation();
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
            }
            else
            {
                leftChara.gameObject.SetActive(false);
                rightChara.gameObject.SetActive(true);
            }
        }

        void WarningAnimation()
        {
            //warningText.rectTransform.DOPunchScale(
            //    transform.localScale * 1.5f, 4.0f).OnComplete(() => EndWarningEvent());
            warningText.rectTransform.DOShakeScale(
                4.0f).OnComplete(() => EndWarningEvent());
        }

        void EndWarningEvent()
        {
            isEventing = false;
            warningText.gameObject.SetActive(false);
            SetNextLine();
        }
    }
}