using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageSelect;

namespace StageSelect
{
    public class StageTable : MonoBehaviour
    {
        public static StageTable instance;

        public static bool[] stageClearedList = new bool[32];
        public static bool[] prevStageClearedList = new bool[32];

        public static int challengeStageId = 0;

        //int stageNum;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                //stageNum = GameObject.Find("StageList").transform.childCount;
                //stageClearedList = new bool[stageNum];
                DontDestroyOnLoad(this);
            }
            else Destroy(gameObject);
            
            /*for(int i = 0; i < 6; i++)
            {
                stageClearedList[i] = true;
            }*/
            //Debug.Log("<color=red>" + stageClearedList[challengeStageId] + "</color>");
        }
    }
}
