using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    enum Item
    {
        BACK = 0,
        AUDIO = 1,
        TITLE = 2,
    }

    [SerializeField]
    private Image frogUI;
    [SerializeField]
    private Image backUI;
    [SerializeField]
    private TextMeshProUGUI backText;
    [SerializeField]
    private TextMeshProUGUI audioText;
    [SerializeField]
    private TextMeshProUGUI titleText;

    public GameObject tearObject;
    public Transform[] tearPosition;

    public Vector3 startPosition = Vector3.zero;
    public Vector3 endPosition = Vector3.zero;
    public float animTime = 1.0f;

    public bool isDisplayed { get; private set; }

    Tween frogTween;
    Tween backTween;

    private int index = 0;
    private int itemNum = 3;

    BaseMenu menuState;

    // Start is called before the first frame update
    void Start()
    {
        frogUI.rectTransform.localPosition = startPosition; // 見えない位置に配置
        isDisplayed = false;

        menuState = new Menu(0, itemNum, this);
    }

    private void Update()
    {
        if (!isDisplayed) return;

        menuState.menuUpdate();
    }

    public void StartMenu()
    {
        isDisplayed = true;

        StartFrogUI();
        StartBackUI();
    }

    public void EndMenu()
    {
        //isDisplayed = false;

        EndFrogUI();
        EndBackUI();
    }

    public void TextApeal(TextMeshProUGUI text)
    {
        text.color = Color.red;
    }

    public void EndTextApeal(TextMeshProUGUI text)
    {
        text.color = Color.white;
    }

    private void StartFrogUI()
    {
        frogTween.Kill();

        //frogUI.gameObject.SetActive(true);
        frogTween = frogUI.rectTransform.DOLocalMove(
            endPosition,
            animTime
            )
            .SetEase(Ease.OutBack);
    }

    private void StartBackUI()
    {
        backTween.Kill();

        //frogUI.gameObject.SetActive(true);
        backTween = DOTween.To(
            () => backUI.GetComponent<Image>().color,
            num => backUI.GetComponent<Image>().color = num,
            new Color(backUI.GetComponent<Image>().color.r, backUI.GetComponent<Image>().color.g,
            backUI.GetComponent<Image>().color.b, 1f),
            animTime
            );
    }

    private void EndFrogUI()
    {
        frogTween.Kill();

        frogTween = frogUI.rectTransform.DOLocalMove(
            startPosition,
            animTime
            ).SetEase(Ease.OutQuint).OnComplete(() => isDisplayed = false);
    }

    private void EndBackUI()
    {
        backTween.Kill();

        backTween = DOTween.To(
            () => backUI.GetComponent<Image>().color,
            num => backUI.GetComponent<Image>().color = num,
            new Color(backUI.GetComponent<Image>().color.r, backUI.GetComponent<Image>().color.g,
            backUI.GetComponent<Image>().color.b, 0f),
            animTime
            );
    }

    private abstract class BaseMenu
    {
        protected MenuController menuCtrl;
        public abstract void menuUpdate();
    }

    private class Menu : BaseMenu
    {
        private int index;
        private int itemNum;

        // コンストラクタはインターバル
        TadaLib.Timer timer = new TadaLib.Timer(1f);

        public Menu(int _index, int _itemNum, MenuController _menuCtrl)
        {
            index = _index;
            _itemNum = itemNum;
            menuCtrl = _menuCtrl;
        }

        public override void menuUpdate()
        {
            if (ActionInput.GetButtonDown(ButtonCode.DownArrow))
                index = (++index) % itemNum;
            else if (ActionInput.GetButtonDown(ButtonCode.UpArrow))
                index = (--index + itemNum) % itemNum;
            else if (ActionInput.GetButtonDown(ButtonCode.Jump))
            {
                switch (index)
                {
                    case (int)Item.BACK:
                        menuCtrl.EndMenu();
                        break;
                    case (int)Item.AUDIO:
                        menuCtrl.menuState = new Audio();
                        break;
                    case (int)Item.TITLE:
                        menuCtrl.menuState = new Title();
                        break;
                }
            }

            switch (index)
            {
                case (int)Item.BACK:
                    menuCtrl.TextApeal(menuCtrl.backText);
                    menuCtrl.EndTextApeal(menuCtrl.audioText);
                    menuCtrl.EndTextApeal(menuCtrl.titleText);
                    break;
                case (int)Item.AUDIO:
                    menuCtrl.TextApeal(menuCtrl.audioText);
                    menuCtrl.EndTextApeal(menuCtrl.backText);
                    menuCtrl.EndTextApeal(menuCtrl.titleText);
                    break;
                case (int)Item.TITLE:
                    menuCtrl.TextApeal(menuCtrl.titleText);
                    menuCtrl.EndTextApeal(menuCtrl.backText);
                    menuCtrl.EndTextApeal(menuCtrl.audioText);
                    timer.TimeUpdate(Time.deltaTime);
                    if (timer.IsTimeout())
                    {
                        Debug.Log("涙でてる");
                        Instantiate(menuCtrl.tearObject, menuCtrl.tearPosition[0].position,
                        Quaternion.identity);

                        Instantiate(menuCtrl.tearObject, menuCtrl.tearPosition[1].position,
                        Quaternion.identity);

                        timer.TimeReset();
                    }
                    break;
            }
        }
    }

    private class Audio : BaseMenu
    {
        public override void menuUpdate()
        {

        }
    }

    private class Title : BaseMenu
    {
        public override void menuUpdate()
        {

        }
    }

}