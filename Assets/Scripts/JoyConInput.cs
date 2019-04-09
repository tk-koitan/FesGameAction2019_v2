using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyConInput : ActionInput
{
    private Joycon stickJoycon;
    private Joycon gyroJoycon;

    protected override void Update()
    {
        if (stickJoycon == null)
        {
            var joycons = JoyconManager.Instance.j;
            stickJoycon = joycons.Find(c => c.isLeft);
            if(stickJoycon == null)
            {
                stickJoycon = joycons.Find(c => !c.isLeft);
            }
        }

        if (gyroJoycon == null)
        {
            var joycons = JoyconManager.Instance.j;
            gyroJoycon = joycons.Find(c => !c.isLeft);
            if (gyroJoycon == null)
            {
                gyroJoycon = joycons.Find(c => c.isLeft);
            }
        }

        foreach(Joycon joycon in JoyconManager.Instance.j)
        {
            if(joycon.GetButton(Joycon.Button.SL) && joycon.GetButton(Joycon.Button.SR))
            {
                gyroJoycon = joycon;
            }
        }

        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    /*
    public override bool GetButton(ButtonCode code)
    {
        if (!actionEnabled) return false;
        switch (code)
        {
            case ButtonCode.Jump:
                return stickJoycon.GetButton(Joycon.Button.DPAD_LEFT);
            case ButtonCode.UpArrow:
                return stickJoycon.GetStick()[0] > 0;
            case ButtonCode.DownArrow:
                return stickJoycon.GetStick()[0] < 0;
            case ButtonCode.LeftArrow:
                return stickJoycon.GetStick()[1] > 0;
            case ButtonCode.RightArrow:
                return stickJoycon.GetStick()[1] < 0;
        }
        return false;
    }

    public override bool GetButtonDown(ButtonCode code)
    {
        if (!actionEnabled) return false;
        switch (code)
        {
            case ButtonCode.Jump:
                return stickJoycon.GetButtonDown(Joycon.Button.DPAD_LEFT);
            case ButtonCode.UpArrow:
                return stickJoycon.GetStick()[0] > 0;
            case ButtonCode.DownArrow:
                return stickJoycon.GetStick()[0] < 0;
            case ButtonCode.LeftArrow:
                return stickJoycon.GetStick()[1] > 0;
            case ButtonCode.RightArrow:
                return stickJoycon.GetStick()[1] < 0;
        }
        return false;
    }

    public override bool GetButtonUp(ButtonCode code)
    {
        if (!actionEnabled) return false;
        switch (code)
        {
            case ButtonCode.Jump:
                return stickJoycon.GetButtonUp(Joycon.Button.DPAD_LEFT);
            case ButtonCode.UpArrow:
                return stickJoycon.GetStick()[0] > 0;
            case ButtonCode.DownArrow:
                return stickJoycon.GetStick()[0] < 0;
            case ButtonCode.LeftArrow:
                return stickJoycon.GetStick()[1] > 0;
            case ButtonCode.RightArrow:
                return stickJoycon.GetStick()[1] < 0;
        }
        return false;
    }

    public override float GetJoyconAngle()
    {
        return gyroJoycon.GetVector().eulerAngles.y;
    }

    public override Vector3 GetJoyconGyro()
    {
        return gyroJoycon.GetGyro();
    }

    public override Vector3 GetJoyconAccel()
    {
        return gyroJoycon.GetAccel();
    }
    */
}
