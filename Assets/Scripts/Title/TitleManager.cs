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
    private AudioMixer audioMixer;

    private float bgmVol;
    private float seVol;
    private float masterVol;
    // Start is called before the first frame update
    void Start()
    {
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
        }

        if (ActionInput.GetButtonDown(ButtonCode.DownArrow))
        {
            nowIndex++;
            nowIndex %= maxIndex;
        }

        if (onSelecteds[nowIndex] != null)
        {
            onSelecteds[nowIndex]();
        }


        if (onCancel != null && ActionInput.GetButtonDown(ButtonCode.Cancel))
        {
            onCancel();
        }


        cursor.transform.localPosition = cursorDefaultPos + Vector3.down * width * nowIndex;
    }

    OnSelected SetButtonPush(Action onPush)
    {
        return () =>
        {
            if (ActionInput.GetButtonDown(ButtonCode.Jump))
            {
                onPush();
            }
        };
    }

    void StartPlacement()
    {
        cursor.gameObject.SetActive(true);
        maxIndex = 4;
        titleUi.text = "スタート\nそうさほうほう\nオプション\nおわる";
        onSelecteds = new OnSelected[maxIndex];
        onSelecteds[0] = SetButtonPush(() => SceneManager.LoadScene("RocketStage"));
        onSelecteds[1] = SetButtonPush(Manual);
        onSelecteds[2] = SetButtonPush(Option);
        onSelecteds[3] = SetButtonPush(Quit);
        onCancel = null;
    }

    void Manual()
    {
        maxIndex = 1;
        nowIndex = 0;
        titleUi.text = "そうさほうほう\nかわずたん\u3000\u3000\u3000\u3000\u3000\u3000\u3000ジョイコン\n<sprite=7>\u3000…いどう\u3000\u3000\u3000\u3000\u3000\u3000 かたむける・ふる\n<sprite=4>\u3000…ジャンプ、決定\u3000\u3000 ZLでほせい\n<sprite=3>\u3000…キャンセル";
        onSelecteds[0] = SetButtonPush(StartPlacement);
        onSelecteds[0] += SetButtonPush(() => nowIndex = 1);
        onCancel = StartPlacement;
        onCancel += () => nowIndex = 1;
        cursor.gameObject.SetActive(false);
    }

    void Option()
    {
        cursor.gameObject.SetActive(true);
        maxIndex = 4;
        nowIndex = 1;
        audioMixer.GetFloat("MasterVol", out masterVol);
        audioMixer.GetFloat("BGMVol", out bgmVol);
        audioMixer.GetFloat("SEVol", out seVol);
        titleUi.text = "おんりょうせってい\n全体 < " + (masterVol + 80) + " >\nBGM < " + (bgmVol + 80) + " >\nこうかおん < " + (seVol + 80) + " >";
        onSelecteds[1] = SetMaster();
        onSelecteds[2] = SetBGM();
        onSelecteds[3] = SetSE();
        onCancel = StartPlacement;
        onCancel += () => nowIndex = 2;
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
            titleUi.text = "おんりょうせってい\n全体 < " + (masterVol + 80) + " >\nBGM < " + (bgmVol + 80) + " >\nこうかおん < " + (seVol + 80) + " >";
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
            titleUi.text = "おんりょうせってい\n全体 < " + (masterVol + 80) + " >\nBGM < " + (bgmVol + 80) + " >\nこうかおん < " + (seVol + 80) + " >";
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
            titleUi.text = "おんりょうせってい\n全体 < " + (masterVol + 80) + " >\nBGM < " + (bgmVol + 80) + " >\nこうかおん < " + (seVol + 80) + " >";
        };
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
