using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStageState : MonoBehaviour
{
    public int stageId = 0;
    public string stageName = "";
    public Sprite stageSprite;
    public Transform stageTransform;

    [SerializeField]
    protected StageState rightStage;
    [SerializeField]
    protected StageState leftStage;
    [SerializeField]
    protected StageState upStage;
    [SerializeField]
    protected StageState downStage;

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
