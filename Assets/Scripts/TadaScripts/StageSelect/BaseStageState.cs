using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageSelect;

namespace StageSelect
{
    public class BaseStageState : MonoBehaviour
    {
        public int stageId = 0;
        public string stageSceneName = "CamTestStage";
        public string stageName = "";
        public Sprite stageSprite;

        [System.Serializable]
        public class NextStage
        {
            [field: SerializeField]
            public StageState upStage { get; private set; }
            [field: SerializeField]
            public StageState rightStage { get; private set; }
            [field: SerializeField]
            public StageState downStage { get; private set; }
            [field: SerializeField]
            public StageState leftStage { get; private set; }
        }

        [System.Serializable]
        public class StagePath
        {
            [field: SerializeField]
            public Transform[] upPath { get; private set; }
            [field: SerializeField]
            public Transform[] rightPath { get; private set; }
            [field: SerializeField]
            public Transform[] downPath { get; private set; }
            [field: SerializeField]
            public Transform[] leftPath { get; private set; }
        }

        [System.Serializable]
        public class CanMove
        {
            public bool up; // setをprotectedにしても継承元で変更できなかった・・・
            public bool right;
            public bool down;
            public bool left;
        }

        public NextStage nextStage;
        public StagePath stagePath;
        public CanMove canMove;

        /*
        public virtual bool goUpStage(ref StageState _nowStage)
        {
            return false;
        }
        public virtual bool goRightStage(ref StageState _nowStage)
        {
            return false;
        }
        public virtual bool goDownStage(ref StageState _nowStage)
        {
            return false;
        }
        public virtual bool goLeftStage(ref StageState _nowStage)
        {
            return false;
        }*/
    }
}