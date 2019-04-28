using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TadaLib;

public class JoyconWarning : MonoBehaviour
{
    Timer timer;

    public float interval = 1;

    // Start is called before the first frame update
    void Start()
    {
        timer = new Timer(interval);
    }

    // Update is called once per frame
    void Update()
    {
        timer.TimeUpdate(Time.deltaTime);
        if (timer.IsTimeout())
        {
            Debug.Log("1秒経ちました");
            timer.TimeReset();
        }
    }
}
