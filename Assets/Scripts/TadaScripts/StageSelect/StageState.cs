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
            SetStageExist();
        }
    }

    private void SetStageExist()
    {
        if (nextStage.upStage != null) canMove.up = true;
        if (nextStage.rightStage != null) canMove.right = true;
        if (nextStage.downStage != null) canMove.down = true;
        if (nextStage.leftStage != null) canMove.left = true;
    }
    /*
    public bool[] GetStageExist()
    {
        return canGoStage;
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
    }*/

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (nextStage.upStage != null)
        {
            Vector3 from = transform.position;
            Vector3 to;
            foreach (Transform pathTransform in stagePath.upPath)
            {
                to = pathTransform.position;
                GizmosExtensions2D.DrawArrow2D(from, to);
                from = pathTransform.position;
            }
            to = nextStage.upStage.transform.position;
            GizmosExtensions2D.DrawArrow2D(from, to);
        }
        if (nextStage.rightStage != null)
        {
            Vector3 from = transform.position;
            Vector3 to;
            foreach (Transform pathTransform in stagePath.rightPath)
            {
                to = pathTransform.position;
                GizmosExtensions2D.DrawArrow2D(from, to);
                from = pathTransform.position;
            }
            to = nextStage.rightStage.transform.position;
            GizmosExtensions2D.DrawArrow2D(from, to);
        }
        if (nextStage.downStage != null)
        {
            Vector3 from = transform.position;
            Vector3 to;
            foreach (Transform pathTransform in stagePath.downPath)
            {
                to = pathTransform.position;
                GizmosExtensions2D.DrawArrow2D(from, to);
                from = pathTransform.position;
            }
            to = nextStage.downStage.transform.position;
            GizmosExtensions2D.DrawArrow2D(from, to);
        }
        if (nextStage.leftStage != null)
        {
            Vector3 from = transform.position;
            Vector3 to;
            foreach (Transform pathTransform in stagePath.leftPath)
            {
                to = pathTransform.position;
                GizmosExtensions2D.DrawArrow2D(from, to);
                from = pathTransform.position;
            }
            to = nextStage.leftStage.transform.position;
            GizmosExtensions2D.DrawArrow2D(from, to);
        }
    }
#endif
}
