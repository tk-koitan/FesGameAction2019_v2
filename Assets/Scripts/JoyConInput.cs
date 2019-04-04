using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyConInput : ActionInput
{
    private Joycon m_joycon;

    protected override void Update()
    {
        if (m_joycon == null)
        {
            var joycons = JoyconManager.Instance.j;
            m_joycon = joycons.Find(c => !c.isLeft);
            if(m_joycon == null)
            {
                m_joycon = joycons.Find(c => c.isLeft);
            }
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
                return m_joycon.GetButton(Joycon.Button.DPAD_RIGHT);
            case ButtonCode.UpArrow:
                return m_joycon.GetStick()[0] < 0;
            case ButtonCode.DownArrow:
                return m_joycon.GetStick()[0] > 0;
            case ButtonCode.LeftArrow:
                return m_joycon.GetStick()[1] < 0;
            case ButtonCode.RightArrow:
                return m_joycon.GetStick()[1] > 0;
        }
        return false;
    }

    public override bool GetButtonDown(ButtonCode code)
    {
        if (!actionEnabled) return false;
        switch (code)
        {
            case ButtonCode.Jump:
                return m_joycon.GetButtonDown(Joycon.Button.DPAD_RIGHT);
            case ButtonCode.UpArrow:
                return m_joycon.GetStick()[0] < 0;
            case ButtonCode.DownArrow:
                return m_joycon.GetStick()[0] > 0;
            case ButtonCode.LeftArrow:
                return m_joycon.GetStick()[1] < 0;
            case ButtonCode.RightArrow:
                return m_joycon.GetStick()[1] > 0;
        }
        return false;
    }

    public override bool GetButtonUp(ButtonCode code)
    {
        if (!actionEnabled) return false;
        switch (code)
        {
            case ButtonCode.Jump:
                return m_joycon.GetButtonUp(Joycon.Button.DPAD_RIGHT);
            case ButtonCode.UpArrow:
                return m_joycon.GetStick()[0] < 0;
            case ButtonCode.DownArrow:
                return m_joycon.GetStick()[0] > 0;
            case ButtonCode.LeftArrow:
                return m_joycon.GetStick()[1] < 0;
            case ButtonCode.RightArrow:
                return m_joycon.GetStick()[1] > 0;
        }
        return false;
    }

    public override float GetJoyconAngle()
    {
        return m_joycon.GetVector().eulerAngles.y;
    }

    public override Vector3 GetJoyconGyro()
    {
        return m_joycon.GetGyro();
    }

    public override Vector3 GetJoyconAccel()
    {
        return m_joycon.GetAccel();
    }
}
