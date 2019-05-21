using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //エディタ上では実行されないので注意
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Screen.SetResolution(640, 360, Screen.fullScreen);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Screen.SetResolution(1280, 720, Screen.fullScreen);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Screen.SetResolution(1440, 810, Screen.fullScreen);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Screen.SetResolution(1920, 1080, Screen.fullScreen);
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("解像度: " + Screen.width + "×" + Screen.height);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
}
