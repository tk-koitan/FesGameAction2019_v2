using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;

public class StageState : BaseStageState
{
    // stageStateが持つべき状態
    // ステージの画像(名前)、座標、上下左右押したときの遷移先ステート
    // 道のりも必要？

    private void Start()
    {
        if (StageSelect.StageTable.stageClearedList[stageId])
        {
            GetComponent<SpriteRenderer>().color =
                new Color(0.2f, 0f, 0f, 1f);
        }
    }

    public override bool goRightStage(ref StageState _nowStage)
    {
        if (rightStage == null) return false;
        _nowStage = rightStage;
        return true;
    }
    public override bool goLeftStage(ref StageState _nowStage)
    {
        if (leftStage == null) return false;
        _nowStage = leftStage;
        return true;
    }
    public override bool goUpStage(ref StageState _nowStage)
    {
        if (upStage == null) return false; ;
        _nowStage = upStage;
        return true;
    }
    public override bool goDownStage(ref StageState _nowStage)
    {
        if (downStage == null) return false;
        _nowStage = downStage;
        return true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(rightStage != null && rightStage.leftStage == this)
        {
            GizmosExtensions2D.DrawArrow2D(stageTransform.position, rightStage.stageTransform.position);
        }
        if(leftStage != null && leftStage.rightStage == this)
        {
            GizmosExtensions2D.DrawArrow2D(stageTransform.position, leftStage.stageTransform.position);
        }
        if (upStage != null && upStage.downStage == this)
        {
            GizmosExtensions2D.DrawArrow2D(stageTransform.position, upStage.stageTransform.position);
        }
        if (downStage != null && downStage.upStage == this)
        {
            GizmosExtensions2D.DrawArrow2D(stageTransform.position, downStage.stageTransform.position);
        }
    }
#endif
}
