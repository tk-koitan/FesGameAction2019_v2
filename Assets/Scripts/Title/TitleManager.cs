using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;

public class TitleManager : MonoBehaviour
{
    public TextMeshProUGUI titleUi;
    public Image cursor;
    public float width;
    private int nowIndex = 0;
    private int maxIndex;
    private int addIndex = 0;
    private Vector3 cursorDefaultPos;

    delegate void OnPush();
    delegate void OnSelected();
    OnSelected[] onSelecteds;
    OnPush onCancel;

    [SerializeField]
    private AudioClip decisionSe;
    [SerializeField]
    private AudioClip cancelSe;
    [SerializeField]
    private AudioClip drumSe;
    [SerializeField]
    private AudioMixer audioMixer;

    private float bgmVol;
    private float seVol;
    private float masterVol;

    private AudioSource audioSource;

    private int ScreenSizeNum = 1;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cursorDefaultPos = cursor.transform.localPosition;
        StartPlacement();
        nowIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (ActionInput.GetButtonDown(ButtonCode.UpArrow))
        {
            nowIndex--;
            nowIndex = (nowIndex + maxIndex) % maxIndex;
            audioSource.PlayOneShot(drumSe);
        }

        if (ActionInput.GetButtonDown(ButtonCode.DownArrow))
        {
            nowIndex++;
            nowIndex %= maxIndex;
            audioSource.PlayOneShot(drumSe);
        }

        if (onSelecteds[nowIndex] != null)
        {
            onSelecteds[nowIndex]();
        }


        if (onCancel != null && ActionInput.GetButtonDown(ButtonCode.Cancel))
        {
            onCancel();
            audioSource.PlayOneShot(cancelSe);
        }


        cursor.transform.localPosition = cursorDefaultPos + Vector3.down * width * (nowIndex + addIndex);
    }

    OnSelected SetButtonPush(Action onPush)
    {
        return () =>
        {
            if (ActionInput.GetButtonDown(ButtonCode.Jump))
            {
                onPush();
                audioSource.PlayOneShot(decisionSe);
            }
        };
    }

    void StartPlacement()
    {
        cursor.gameObject.SetActive(true);
        maxIndex = 4;
        addIndex = 0;
        titleUi.text = "スタート\nそうさほうほう\nオプション\nおわる";
        onSelecteds = new OnSelected[maxIndex];
        onSelecteds[0] = SetButtonPush(StartRocketScene);
        onSelecteds[1] = SetButtonPush(Manual);
        onSelecteds[2] = SetButtonPush(Option);
        onSelecteds[3] = SetButtonPush(Quit);
        onCancel = null;
    }

    void StartRocketScene()
    {
        if (!FadeManager.Instance.isFading)
        {
            FadeManager.Instance.LoadScene("RocketStage", 1f);
        }
    }

    void Manual()
    {
        maxIndex = 1;
        nowIndex = 0;
        titleUi.text = "そうさほうほう\nかわずたん\u3000\u3000\u3000\u3000\u3000\u3000\u3000ジョイコン\n<sprite=7>\u3000…いどう\u3000\u3000\u3000\u3000\u3000\u3000 かたむける・ふる\n<sprite=4>\u3000…ジャンプ、決定\u3000\u3000 ZRでほせい\n<sprite=3>\u3000…キャンセル";
        onSelecteds[0] = SetButtonPush(StartPlacement);
        onSelecteds[0] += SetButtonPush(() => nowIndex = 1);
        onCancel = StartPlacement;
        onCancel += () => nowIndex = 1;
        cursor.gameObject.SetActive(false);
    }

    void Option()
    {
        cursor.gameObject.SetActive(true);
        titleUi.text = "オプション\nがめんせってい\nおんりょうせってい";
        maxIndex = 2;
        nowIndex = 0;
        addIndex = 1;
        onSelecteds[0] = SetButtonPush(VideoOption);
        onSelecteds[1] = SetButtonPush(BgmOption);
        onCancel = StartPlacement;
        onCancel += () => nowIndex = 2;
    }

    void VideoOption()
    {
        cursor.gameObject.SetActive(true);
        maxIndex = 2;
        nowIndex = 0;
        addIndex = 1;
        titleUi.text = "がめんせってい\nフルスクリーン\u3000< " + ScreenIsFull() + " >\nかいぞうど < " + ScreenSizeString() + " >";
        onSelecteds[0] = SetFullScreen();
        onSelecteds[1] = SetScreenSize();
        onCancel = Option;
        onCancel += () => nowIndex = 0;
    }

    void BgmOption()
    {
        cursor.gameObject.SetActive(true);
        maxIndex = 4;
        nowIndex = 0;
        addIndex = 1;
        audioMixer.GetFloat("MasterVol", out masterVol);
        audioMixer.GetFloat("BGMVol", out bgmVol);
        audioMixer.GetFloat("SEVol", out seVol);
        titleUi.text = "おんりょうせってい … <sprite=7>でへんこう\n全体 < " + (masterVol + 80) + " >\nBGM < " + (bgmVol + 80) + " >\nこうかおん < " + (seVol + 80) + " >\n元にもどす";
        onSelecteds[0] = SetMaster();
        onSelecteds[1] = SetBGM();
        onSelecteds[2] = SetSE();
        onSelecteds[3] = SetDefault();
        onCancel = Option;
        onCancel += () => nowIndex = 1;
    }

    OnSelected SetMaster()
    {
        return () =>
        {
            if (ActionInput.GetButton(ButtonCode.RightArrow))
            {
                masterVol++;
                masterVol = Mathf.Min(masterVol, 20);
                audioMixer.SetFloat("MasterVol", masterVol);
            }
            if (ActionInput.GetButton(ButtonCode.LeftArrow))
            {
                masterVol--;
                masterVol = Math.Max(masterVol, -80);
                audioMixer.SetFloat("MasterVol", masterVol);
            }
            titleUi.text = "おんりょうせってい … <sprite=7>でへんこう\n全体 < " + (masterVol + 80) + " >\nBGM < " + (bgmVol + 80) + " >\nこうかおん < " + (seVol + 80) + " >\n元にもどす";
        };
    }

    OnSelected SetBGM()
    {
        return () =>
        {
            if (ActionInput.GetButton(ButtonCode.RightArrow))
            {
                bgmVol++;
                bgmVol = Mathf.Min(bgmVol, 20);
                audioMixer.SetFloat("BGMVol", bgmVol);
            }
            if (ActionInput.GetButton(ButtonCode.LeftArrow))
            {
                bgmVol--;
                bgmVol = Math.Max(bgmVol, -80);
                audioMixer.SetFloat("BGMVol", bgmVol);
            }
            titleUi.text = "おんりょうせってい … <sprite=7>でへんこう\n全体 < " + (masterVol + 80) + " >\nBGM < " + (bgmVol + 80) + " >\nこうかおん < " + (seVol + 80) + " >\n元にもどす";
        };
    }

    OnSelected SetSE()
    {
        return () =>
        {
            if (ActionInput.GetButton(ButtonCode.RightArrow))
            {
                seVol++;
                seVol = Mathf.Min(seVol, 20);
                audioMixer.SetFloat("SEVol", seVol);
            }
            if (ActionInput.GetButton(ButtonCode.LeftArrow))
            {
                seVol--;
                seVol = Math.Max(seVol, -80);
                audioMixer.SetFloat("SEVol", seVol);
            }
            titleUi.text = "おんりょうせってい … <sprite=7>でへんこう\n全体 < " + (masterVol + 80) + " >\nBGM < " + (bgmVol + 80) + " >\nこうかおん < " + (seVol + 80) + " >\n元にもどす";
        };
    }

    OnSelected SetFullScreen()
    {
        return () =>
        {
            if (ActionInput.GetButtonDown(ButtonCode.RightArrow) || ActionInput.GetButtonDown(ButtonCode.LeftArrow))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
            titleUi.text = "がめんせってい\nフルスクリーン\u3000< " + ScreenIsFull() + " >\nかいぞうど < " + ScreenSizeString() + " >";
        };
    }

    string ScreenIsFull()
    {
        if (Screen.fullScreen)
        {
            return "ON";
        }
        else return "OFF";
    }

    OnSelected SetScreenSize()
    {
        return () =>
        {
            if (ActionInput.GetButtonDown(ButtonCode.RightArrow))
            {
                ScreenSizeNum++;
                ScreenSizeNum = (ScreenSizeNum + 4) % 4;
                ScreenSizeChange();
            }
            if(ActionInput.GetButtonDown(ButtonCode.LeftArrow))
            {
                ScreenSizeNum--;
                ScreenSizeNum = (ScreenSizeNum + 4) % 4;
                ScreenSizeChange();
            }
            titleUi.text = "がめんせってい\nフルスクリーン\u3000< " + ScreenIsFull() + " >\nかいぞうど < "+ ScreenSizeString() +" >";
        };
    }

    string ScreenSizeString()
    {
        switch(ScreenSizeNum)
        {
            case 0:
                return "640 × 360";
            case 1:
                return "1280 × 720";
            case 2:
                return "1440 × 810";
            case 3:
                return "1920 × 1080";
        }
        return "";
    }

    void ScreenSizeChange()
    {
        switch (ScreenSizeNum)
        {
            case 0:
                Screen.SetResolution(640, 360, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(1440, 810, Screen.fullScreen);
                break;
            case 3:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
        }
    }

    /*
    OnSelected SetVol(string n)
    {
        return () =>
        {
            out float vol = 0;
            switch (n)
            {
                case "MasterVol":
                    vol = masterVol;
                    break;
                case "BGMVol":
                    vol = bgmVol;
                    break;
                case "SEVol":
                    vol = seVol;
                    break;
            }
            if (ActionInput.GetButton(ButtonCode.RightArrow))
            {
                vol++;
                vol = Mathf.Min(vol, 20);
                audioMixer.SetFloat(n, vol);
            }
            if (ActionInput.GetButton(ButtonCode.LeftArrow))
            {
                vol--;
                vol = Math.Max(vol, -80);
                audioMixer.SetFloat(n, vol);
            }
            titleUi.text = "おんりょうせってい … <sprite=7>でへんこう\n全体 < " + (masterVol + 80) + " >\nBGM < " + (bgmVol + 80) + " >\nこうかおん < " + (seVol + 80) + " >\n元にもどす";
        };
    }
    */

    OnSelected SetDefault()
    {
        return SetButtonPush(() =>
        {
            masterVol = bgmVol = seVol = 0;
            audioMixer.SetFloat("MasterVol", masterVol);
            audioMixer.SetFloat("BGMVol", bgmVol);
            audioMixer.SetFloat("SEVol", seVol);
            titleUi.text = "おんりょうせってい … <sprite=7>でへんこう\n全体 < " + (masterVol + 80) + " >\nBGM < " + (bgmVol + 80) + " >\nこうかおん < " + (seVol + 80) + " >\n元にもどす";
        });
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
    }
}
