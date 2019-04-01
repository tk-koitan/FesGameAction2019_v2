using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stageSelect
{
    public class StageTable : MonoBehaviour
    {
        public enum StageName{
            Elementary = 1,
            b = 2,
            c = 3,
            d = 4,
            e = 5,
            f = 6,
            g = 7,
            h = 8,
            i = 9,
            j = 10,
            k = 11
        };

        public StageState[] stageStateTable;
    }
}
