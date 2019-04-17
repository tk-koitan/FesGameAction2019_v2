using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStageState : MonoBehaviour
{
    public int stageId = 0;
    public string stageSceneName = "CamTestStage";
    public string stageName = "";
    public Sprite stageSprite;
    public Transform stageTransform;

    protected bool[] stageExist = new bool[4] {false, false, false, false };

    [SerializeField]
    protected StageState rightStage;
    [SerializeField]
    protected StageState leftStage;
    [SerializeField]
    protected StageState upStage;
    [SerializeField]
    protected StageState downStage;

    // 道のりを取得
    public Transform[] fromLeftPath;
    public Transform[] fromRightPath;
    public Transform[] fromUpPath;
    public Transform[] fromDownPath;


    public virtual bool goRightStage(ref StageState _nowStage)
    {
        return false;
    }
    public virtual bool goLeftStage(ref StageState _nowStage)
    {
        return false;
    }
    public virtual bool goUpStage(ref StageState _nowStage)
    {
        return false;
    }
    public virtual bool goDownStage(ref StageState _nowStage)
    {
        return false;
    }
}
