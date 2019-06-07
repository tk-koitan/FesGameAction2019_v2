using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using StageSelect;
using UnityEngine.SceneManagement;

namespace StageSelect
{
    public class MenuController : MonoBehaviour
    {
        enum Item
        {
            BACK = 0,
            AUDIO = 1,
            TITLE = 2,
        }
        enum ItemAudio
        {
            BACK = 0,
            BGM = 1,
            SE = 2,
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

        [SerializeField]
        private Image frogUIAudio;
        [SerializeField]
        private Image backUIAudio;
        [SerializeField]
        private TextMeshProUGUI backTextAudio;
        [SerializeField]
        private TextMeshProUGUI bgmText;
        [SerializeField]
        private TextMeshProUGUI seText;

        public GameObject tearObject;
        public Transform[] tearPosition;

        [SerializeField]
        private AudioClip calcelSE;
        [SerializeField]
        private AudioClip decideSE;
        private AudioSource audioSource;

        public Vector3 startRotation = Vector3.zero;
        public Vector3 endRotation = Vector3.zero;
        public float animTime = 1.0f;

        public bool isDisplayed { get; private set; }

        Tween frogTween;
        Tween backTween;
        Tween frogTweenAudio;
        Tween backTweenAudio;

        private int index = 0;
        private int itemNum = 3;

        BaseMenu menuState;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            isDisplayed = false;

            //menuState = new Menu(0, itemNum, this);
        }

        private void Update()
        {
            if (!isDisplayed) return;

            if(menuState != null)
                menuState.menuUpdate();
        }

        public void StartMenu()
        {
            isDisplayed = true;

            menuState = new Menu(0, itemNum, this);

            StartFrogUI(frogUI, frogTween);
            StartBackUI(backUI, backTween);
            audioSource.PlayOneShot(decideSE);
        }

        public void EndMenu()
        {
            //isDisplayed = false;

            EndFrogUI(frogUI, frogTween);
            EndBackUI(backUI, backTween);
            audioSource.PlayOneShot(calcelSE);
        }

        public void TextApeal(TextMeshProUGUI text)
        {
            text.color = Color.red;
        }

        public void EndTextApeal(TextMeshProUGUI text)
        {
            text.color = Color.white;
        }

        private void StartFrogUI(Image frogImage, Tween tween)
        {
            tween.Kill();

            //frogUI.gameObject.SetActive(true);
            tween = frogImage.rectTransform.DOLocalRotate(
                endRotation,
                animTime
                )
                .SetEase(Ease.OutBack);
        }

        private void StartBackUI(Image backImage, Tween tween)
        {
            tween.Kill();

            //frogUI.gameObject.SetActive(true);
            tween = DOTween.To(
                () => backImage.GetComponent<Image>().color,
                num => backImage.GetComponent<Image>().color = num,
                new Color(backImage.GetComponent<Image>().color.r, backImage.GetComponent<Image>().color.g,
                backImage.GetComponent<Image>().color.b, 1f),
                animTime
                );
        }

        private void EndFrogUI(Image frogImage, Tween tween, bool changeDisplay = false)
        {
            tween.Kill();

            tween = frogImage.rectTransform.DOLocalRotate(
                startRotation,
                animTime
                ).SetEase(Ease.OutQuint).OnComplete(() => isDisplayed = changeDisplay);
        }

        private void EndBackUI(Image backImage, Tween tween)
        {
            tween.Kill();

            tween = DOTween.To(
                () => backImage.GetComponent<Image>().color,
                num => backImage.GetComponent<Image>().color = num,
                new Color(backImage.GetComponent<Image>().color.r, backImage.GetComponent<Image>().color.g,
                backImage.GetComponent<Image>().color.b, 0f),
                animTime
                );
        }

        private abstract class BaseMenu
        {
            protected MenuController menuCtrl;
            public abstract void menuStart();
            public abstract void menuUpdate();
        }

        private class Menu : BaseMenu
        {
            private int index = 0;
            private bool isChanged = true;
            private int itemNum = 3;

            // コンストラクタはインターバル
            TadaLib.Timer timer = new TadaLib.Timer(1f);

            public Menu(int _index, int _itemNum, MenuController _menuCtrl)
            {
                index = _index;
                _itemNum = itemNum;
                menuCtrl = _menuCtrl;
            }

            public override void menuStart()
            {
            }

            public override void menuUpdate()
            {
                if (ActionInput.GetButtonDown(ButtonCode.DownArrow))
                {
                    index = (++index) % itemNum;
                    isChanged = true;
                }
                else if (ActionInput.GetButtonDown(ButtonCode.UpArrow))
                {
                    index = (--index + itemNum) % itemNum;
                    isChanged = true;
                }
                else if (ActionInput.GetButtonDown(ButtonCode.Jump))
                {
                    switch (index)
                    {
                        case (int)Item.BACK:
                            menuCtrl.EndMenu();
                            menuCtrl.menuState = null;
                            break;
                        case (int)Item.AUDIO:
                            menuCtrl.menuState = new Audio(menuCtrl);
                            menuCtrl.menuState.menuStart();
                            break;
                        case (int)Item.TITLE:
                            SceneManager.LoadScene("Title");
                            //menuCtrl.menuState = new Title();
                            break;
                    }
                }

                switch (index)
                {
                    case (int)Item.BACK:
                        if (isChanged)
                        {
                            menuCtrl.TextApeal(menuCtrl.backText);
                            menuCtrl.EndTextApeal(menuCtrl.audioText);
                            menuCtrl.EndTextApeal(menuCtrl.titleText);
                            isChanged = false;
                        }
                        break;
                    case (int)Item.AUDIO:
                        if (isChanged)
                        {
                            menuCtrl.TextApeal(menuCtrl.audioText);
                            menuCtrl.EndTextApeal(menuCtrl.backText);
                            menuCtrl.EndTextApeal(menuCtrl.titleText);
                            isChanged = false;
                        }
                        break;
                    case (int)Item.TITLE:
                        if (isChanged)
                        {
                            menuCtrl.TextApeal(menuCtrl.titleText);
                            menuCtrl.EndTextApeal(menuCtrl.backText);
                            menuCtrl.EndTextApeal(menuCtrl.audioText);
                            isChanged = false;
                        }
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
            private int index = 0;
            private bool isChanged = true;
            private int itemNum = 3;

            public Audio(MenuController _menuCtrl)
            {
                menuCtrl = _menuCtrl;
            }

            public override void menuStart()
            {
                menuCtrl.StartFrogUI(menuCtrl.frogUIAudio, menuCtrl.frogTweenAudio);
                menuCtrl.StartBackUI(menuCtrl.backUIAudio, menuCtrl.backTweenAudio);
            }

            public override void menuUpdate()
            {
                if (ActionInput.GetButtonDown(ButtonCode.DownArrow))
                {
                    index = (++index) % itemNum;
                    isChanged = true;
                }
                else if (ActionInput.GetButtonDown(ButtonCode.UpArrow))
                {
                    index = (--index + itemNum) % itemNum;
                    isChanged = true;
                }
                else if (ActionInput.GetButtonDown(ButtonCode.Jump))
                {
                    switch (index)
                    {
                        case (int)ItemAudio.BACK:
                            menuCtrl.EndFrogUI(menuCtrl.frogUIAudio, menuCtrl.frogTweenAudio, true);
                            menuCtrl.EndBackUI(menuCtrl.backUIAudio, menuCtrl.backTweenAudio);
                            menuCtrl.menuState = new Menu(1, 3, menuCtrl);
                            break;
                        case (int)ItemAudio.BGM:
                            //menuCtrl.menuState = new Audio(menuCtrl);
                            //menuCtrl.menuState.menuStart();
                            break;
                        case (int)ItemAudio.SE:
                            //menuCtrl.menuState = new Title();
                            break;
                    }
                }

                switch (index)
                {
                    case (int)ItemAudio.BACK:
                        if (isChanged)
                        {
                            menuCtrl.TextApeal(menuCtrl.backTextAudio);
                            menuCtrl.EndTextApeal(menuCtrl.bgmText);
                            menuCtrl.EndTextApeal(menuCtrl.seText);
                            isChanged = false;
                        }
                        break;
                    case (int)ItemAudio.BGM:
                        if (isChanged)
                        {
                            menuCtrl.TextApeal(menuCtrl.bgmText);
                            menuCtrl.EndTextApeal(menuCtrl.backTextAudio);
                            menuCtrl.EndTextApeal(menuCtrl.seText);
                            isChanged = false;
                        }
                        break;
                    case (int)ItemAudio.SE:
                        if (isChanged)
                        {
                            menuCtrl.TextApeal(menuCtrl.seText);
                            menuCtrl.EndTextApeal(menuCtrl.backTextAudio);
                            menuCtrl.EndTextApeal(menuCtrl.bgmText);
                            isChanged = false;
                        }
                        break;
                }
            }
        }

        private class Title : BaseMenu
        {
            public override void menuStart()
            {
            }

            public override void menuUpdate()
            {

            }
        }
    }
}