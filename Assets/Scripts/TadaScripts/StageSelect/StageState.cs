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

        SetStageExist();
    }

    private void SetStageExist()
    {
        if (upStage != null) stageExist[0] = true;
        if (rightStage != null) stageExist[1] = true;
        if (downStage != null) stageExist[2] = true;
        if (leftStage != null) stageExist[3] = true;
    }

    public bool[] GetStageExist()
    {
        return stageExist;
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
        if (rightStage != null)
        {
            Vector3 from = rightStage.stageTransform.position;
            Vector3 to;
            foreach (Transform pathTransform in fromRightPath)
            {
                to = pathTransform.position;
                GizmosExtensions2D.DrawArrow2D(from, to);
                from = pathTransform.position;
            }
            to = stageTransform.position;
            GizmosExtensions2D.DrawArrow2D(from, to);
        }
        if (leftStage != null)
        {
            Vector3 from = leftStage.stageTransform.position;
            Vector3 to;
            foreach (Transform pathTransform in fromLeftPath)
            {
                to = pathTransform.position;
                GizmosExtensions2D.DrawArrow2D(from, to);
                from = pathTransform.position;
            }
            to = stageTransform.position;
            GizmosExtensions2D.DrawArrow2D(from, to);
        }
        if (upStage != null)
        {
            Vector3 from = upStage.stageTransform.position;
            Vector3 to;
            foreach (Transform pathTransform in fromUpPath)
            {
                to = pathTransform.position;
                GizmosExtensions2D.DrawArrow2D(from, to);
                from = pathTransform.position;
            }
            to = stageTransform.position;
            GizmosExtensions2D.DrawArrow2D(from, to);
        }
        if (downStage != null)
        {
            Vector3 from = downStage.stageTransform.position;
            Vector3 to;
            foreach (Transform pathTransform in fromDownPath)
            {
                to = pathTransform.position;
                GizmosExtensions2D.DrawArrow2D(from, to);
                from = pathTransform.position;
            }
            to = stageTransform.position;
            GizmosExtensions2D.DrawArrow2D(from, to);
        }
    }
#endif
}
