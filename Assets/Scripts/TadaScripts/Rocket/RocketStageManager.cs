using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class RocketStageManager : MonoBehaviour
{
    [SerializeField]
    private MeteoSponer meteoSponer;
    [SerializeField]
    private RocketController rocketCtrl;
    [SerializeField]
    private TextController textCtrl;

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
        if(textCtrl.isConversationing)
            textCtrl.ScenarioUpdate();
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
        feedInObj(backImage, 1.0f);

        yield return new WaitForSeconds(1.0f);

        explainText.text = "このままだといん石にぶつかる！";
        feedInObj(explainText, 0.5f);

        yield return new WaitForSeconds(0.5f + 1.0f);

        feedOutObj(explainText, 0.3f);

        yield return new WaitForSeconds(0.3f);

        explainText.text = "ジョイコンでロケットをそうじゅうしよう！";
        feedInObj(explainText, 0.5f);

        yield return new WaitForSeconds(0.5f + 1.0f);

        feedOutObj(explainText, 0.3f);

        leftDistanceText.text = rocketCtrl.leftDistance.ToString();

        yield return new WaitForSeconds(0.3f);

        feedOutObj(backImage, 1.0f);
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
    }



    private void feedInObj(Image target, float time)
    {
        DOTween.To(
            () => target.color,
            num => target.color = num,
            new Color(target.color.r, target.color.g, target.color.b, 0.6f),
            time
            ).SetEase(Ease.InOutCubic);
    }
    private void feedInObj(TextMeshProUGUI target, float time)
    {
        DOTween.To(
            () => target.color,
            num => target.color = num,
            new Color(target.color.r, target.color.g, target.color.b, 0.8f),
            time
            ).SetEase(Ease.InOutCubic);
    }

    private void feedOutObj(Image target, float time)
    {
        DOTween.To(
            () => target.color,
            num => target.color = num,
            new Color(target.color.r, target.color.g, target.color.b, 0.0f),
            time
            ).SetEase(Ease.InOutCubic);
    }
    private void feedOutObj(TextMeshProUGUI target, float time)
    {
        DOTween.To(
            () => target.color,
            num => target.color = num,
            new Color(target.color.r, target.color.g, target.color.b, 0.0f),
            time
            ).SetEase(Ease.InOutCubic);
    }
}
