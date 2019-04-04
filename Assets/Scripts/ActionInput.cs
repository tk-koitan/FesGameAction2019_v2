using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ActionInput : MonoBehaviour
{
    public static bool actionEnabled;
    public abstract bool GetButtonDown(ButtonCode code);
    public abstract bool GetButton(ButtonCode code);
    public abstract bool GetButtonUp(ButtonCode code);

    public abstract float GetJoyconAngle();
    public abstract Vector3 GetJoyconGyro();
    public abstract Vector3 GetJoyconAccel();

    static ActionInput instance;
    public static ActionInput Instatnce
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }
}

public enum ButtonCode
{
    Jump,
    UpArrow,
    DownArrow,
    LeftArrow,
    RightArrow,
}
