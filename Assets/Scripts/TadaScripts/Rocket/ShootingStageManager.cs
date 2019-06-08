using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace RocketStage
{
    public class ShootingStageManager : MonoBehaviour
    {
        [SerializeField]
        private LastBossController lastBossCtrl;
        [SerializeField]
        private BaseRocketController rocketCtrl;
        [SerializeField]
        private TextController textCtrl;
        [SerializeField]
        private TextController textCtrl2;

        [SerializeField]
        private Image backImage;
        [SerializeField]
        private TextMeshProUGUI explainText;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(GameFlow());
        }

        // Update is called once per frame
        void Update()
        {
            if (textCtrl.isConversationing)
                textCtrl.ScenarioUpdate();
            if (textCtrl2.isConversationing)
                textCtrl2.ScenarioUpdate();
        }

        private IEnumerator GameFlow()
        {
            rocketCtrl.DoBeginAnitmation1();

            yield return new WaitForSeconds(rocketCtrl.startAnimationTime);

            textCtrl.ScenarioStart();

            while (textCtrl.isConversationing)
            {
                yield return new WaitForSeconds(1.0f);
            }

            rocketCtrl.DoBeginAnimation2();

            yield return new WaitForSeconds(rocketCtrl.startAnimationTime / 4f);

            lastBossCtrl.BeginAnimation();

            // ここで第三者のコメント

            MusicManager.Play(MusicManager.Instance.bgm5);


            // バックを暗くする
            feedObj(backImage, 1.0f, 0.7f);


            yield return new WaitForSeconds(1.0f);

            explainText.text = "わるものがあらわれた";
            feedObj(explainText, 0.5f, 0.8f);

            yield return new WaitForSeconds(0.5f + 1.5f);

            feedObj(explainText, 0.3f, 0.0f);

            yield return new WaitForSeconds(0.3f);

            explainText.text = "強くなったロケットで、げきたいしよう！";
            feedObj(explainText, 0.5f, 0.8f);

            yield return new WaitForSeconds(0.5f + 1.5f);

            feedObj(explainText, 0.3f, 0.0f);

            yield return new WaitForSeconds(0.3f);

            explainText.text = "レーザーをうてるようになったぞ！";
            feedObj(explainText, 0.5f, 0.8f);

            yield return new WaitForSeconds(0.5f + 1.5f);

            feedObj(explainText, 0.3f, 0.0f);

            yield return new WaitForSeconds(0.3f);

            feedObj(backImage, 1.0f, 0.0f);

            // HPを表示する
            rocketCtrl.GetComponent<ShootingRocketController>().ShowHpImage();
            lastBossCtrl.ShowHpImage();

            yield return new WaitForSeconds(1.0f);

            // ゲーム開始
            rocketCtrl.actionEnabled = true;

            yield return new WaitForSeconds(2.0f);

            lastBossCtrl.actionEnabled = true;

            while (rocketCtrl.isDead == false && lastBossCtrl.isDead == false)
            {
                yield return new WaitForSeconds(1.0f);
            }

            if (rocketCtrl.isDead)
            {
                // バックを暗くする
                feedObj(backImage, 1.0f, 0.7f);

                yield return new WaitForSeconds(1.0f);

                explainText.text = "これは悪いゆめ・・";
                feedObj(explainText, 0.5f, 0.8f);

                yield return new WaitForSeconds(0.5f + 1.5f);

                FadeManager.Instance.LoadScene("KawazStageSelect", 2.0f);
            }
            else // クリア
            {
                rocketCtrl.actionEnabled = false;

                lastBossCtrl.EndAnimation();

                textCtrl2.ScenarioStart();

                while (textCtrl2.isConversationing)
                {
                    yield return new WaitForSeconds(1.0f);
                }

                rocketCtrl.GetComponent<ShootingRocketController>().DoEndAnimation();

                yield return new WaitForSeconds(rocketCtrl.startAnimationTime);

                FadeManager.Instance.LoadScene("StaffRoll", 2.0f);
            }
        }

        private void feedObj(Image target, float time, float endValue)
        {
            DOTween.To(
                () => target.color,
                num => target.color = num,
                new Color(target.color.r, target.color.g, target.color.b, endValue),
                time
                ).SetEase(Ease.InOutCubic);
        }
        private void feedObj(TextMeshProUGUI target, float time, float endValue)
        {
            DOTween.To(
                () => target.color,
                num => target.color = num,
                new Color(target.color.r, target.color.g, target.color.b, endValue),
                time
                ).SetEase(Ease.InOutCubic);
        }
    }
}