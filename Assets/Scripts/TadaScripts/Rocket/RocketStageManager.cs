using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using RocketStage;
using UnityEngine.SceneManagement;

namespace RocketStage
{
    public class RocketStageManager : MonoBehaviour
    {
        [SerializeField]
        private MeteoSponer meteoSponer;
        [SerializeField]
        private RocketController rocketCtrl;
        [SerializeField]
        private TextController textCtrl;
        [SerializeField]
        private TextController textCtrl2;

        [SerializeField]
        private Image backImage;
        [SerializeField]
        private TextMeshProUGUI explainText;
        [SerializeField]
        private TextMeshProUGUI leftDistanceText;

        private


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
            if (rocketCtrl.actionEnabled)
                leftDistanceText.text = rocketCtrl.leftDistance.ToString();
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

            // ここで第三者のコメント

            // バックを暗くする
            feedObj(backImage, 1.0f, 0.7f);

            yield return new WaitForSeconds(1.0f);

            explainText.text = "このままだといん石にぶつかる！";
            feedObj(explainText, 0.5f, 0.8f);

            yield return new WaitForSeconds(0.5f + 1.5f);

            feedObj(explainText, 0.3f, 0.0f);

            yield return new WaitForSeconds(0.3f);

            explainText.text = "ジョイコンでロケットをそうじゅうしよう！";
            feedObj(explainText, 0.5f, 0.8f);

            yield return new WaitForSeconds(0.5f + 1.5f);

            feedObj(explainText, 0.3f, 0.0f);

            leftDistanceText.text = rocketCtrl.leftDistance.ToString();

            yield return new WaitForSeconds(0.3f);

            feedObj(backImage, 1.0f, 0.0f);
            /*
            feedInObj(leftDistanceText, 1.0f);
            foreach(Transform child in transform)
            {
                feedInObj(child.GetComponent<TextMeshProUGUI>(), 1.0f);
            }*/

            yield return new WaitForSeconds(1.0f);

            // ゲーム開始
            meteoSponer.gameObject.SetActive(true);
            rocketCtrl.actionEnabled = true;
            leftDistanceText.gameObject.SetActive(true);

            while (rocketCtrl.isDead == false)
            {
                yield return new WaitForSeconds(1.0f);
            }


            textCtrl2.ScenarioStart();

            while (textCtrl2.isConversationing)
            {
                yield return new WaitForSeconds(1.0f);
            }

            yield return new WaitForSeconds(1.0f);

            SceneManager.LoadScene("ArriveStageSelect");
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