using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TadaLib
{
    public class Timer
    {
        private float time; // 現在の時間
        private float limitTime; // 制限時間

        public Timer(float _limitTime)
        {
            limitTime = _limitTime;
        }

        public void TimeUpdate(float deltaTime)
        {
            time += deltaTime;
        }

        public void TimeReset()
        {
            time = 0;
        }

        public bool IsTimeout()
        {
            return time >= limitTime;
        }
    }
}