using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyConInput : ActionInput
{
    private Joycon m_joyconR;

    protected override void Update()
    {
        if (m_joyconR == null)
        {
            var joycons = JoyconManager.Instance.j;
            m_joyconR = joycons.Find(c => !c.isLeft);
            return;
        }
        base.Update();
    }


    public override bool GetButton(ButtonCode code)
    {
        if (!actionEnabled) return false;
        switch (code)
        {
            case ButtonCode.Jump:
                return m_joyconR.GetButton(Joycon.Button.DPAD_RIGHT);
            case ButtonCode.UpArrow:
                return m_joyconR.GetStick()[0] < 0;
            case ButtonCode.DownArrow:
                return m_joyconR.GetStick()[0] > 0;
            case ButtonCode.LeftArrow:
                return m_joyconR.GetStick()[1] < 0;
            case ButtonCode.RightArrow:
                return m_joyconR.GetStick()[1] > 0;
        }
        return false;
    }

    public override bool GetButtonDown(ButtonCode code)
    {
        if (!actionEnabled) return false;
        switch (code)
        {
            case ButtonCode.Jump:
                return m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT);
            case ButtonCode.UpArrow:
                return m_joyconR.GetStick()[0] < 0;
            case ButtonCode.DownArrow:
                return m_joyconR.GetStick()[0] > 0;
            case ButtonCode.LeftArrow:
                return m_joyconR.GetStick()[1] < 0;
            case ButtonCode.RightArrow:
                return m_joyconR.GetStick()[1] > 0;
        }
        return false;
    }

    public override bool GetButtonUp(ButtonCode code)
    {
        if (!actionEnabled) return false;
        switch (code)
        {
            case ButtonCode.Jump:
                return m_joyconR.GetButtonUp(Joycon.Button.DPAD_RIGHT);
            case ButtonCode.UpArrow:
                return m_joyconR.GetStick()[0] < 0;
            case ButtonCode.DownArrow:
                return m_joyconR.GetStick()[0] > 0;
            case ButtonCode.LeftArrow:
                return m_joyconR.GetStick()[1] < 0;
            case ButtonCode.RightArrow:
                return m_joyconR.GetStick()[1] > 0;
        }
        return false;
    }

    public override float GetJoyconAngle()
    {
        return m_joyconR.GetVector().eulerAngles.y;
    }

    public override Vector3 GetJoyconGyro()
    {
        return m_joyconR.GetGyro();
    }
}
