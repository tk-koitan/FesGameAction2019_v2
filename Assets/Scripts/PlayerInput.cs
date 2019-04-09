using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : ActionInput
{
    private float joyconAngle = 0;
    /*
    public override bool GetButton(ButtonCode code)
    {
        if (!actionEnabled) return false;
        switch(code)
        {
            case ButtonCode.Jump:
                return Input.GetKey(KeyCode.Space);
            case ButtonCode.UpArrow:
                return Input.GetKey(KeyCode.UpArrow);
            case ButtonCode.DownArrow:
                return Input.GetKey(KeyCode.DownArrow);
            case ButtonCode.LeftArrow:
                return Input.GetKey(KeyCode.LeftArrow);
            case ButtonCode.RightArrow:
                return Input.GetKey(KeyCode.RightArrow);
        }
        return false;
    }

    public override bool GetButtonDown(ButtonCode code)
    {
        if (!actionEnabled) return false;
        switch (code)
        {
            case ButtonCode.Jump:
                return Input.GetKeyDown(KeyCode.Space);
            case ButtonCode.UpArrow:
                return Input.GetKeyDown(KeyCode.UpArrow);
            case ButtonCode.DownArrow:
                return Input.GetKeyDown(KeyCode.DownArrow);
            case ButtonCode.LeftArrow:
                return Input.GetKeyDown(KeyCode.LeftArrow);
            case ButtonCode.RightArrow:
                return Input.GetKeyDown(KeyCode.RightArrow);
        }
        return false;
    }

    public override bool GetButtonUp(ButtonCode code)
    {
        if (!actionEnabled) return false;
        switch (code)
        {
            case ButtonCode.Jump:
                return Input.GetKeyUp(KeyCode.Space);
            case ButtonCode.UpArrow:
                return Input.GetKeyUp(KeyCode.UpArrow);
            case ButtonCode.DownArrow:
                return Input.GetKeyUp(KeyCode.DownArrow);
            case ButtonCode.LeftArrow:
                return Input.GetKeyUp(KeyCode.LeftArrow);
            case ButtonCode.RightArrow:
                return Input.GetKeyUp(KeyCode.RightArrow);
        }
        return false;
    }

    public override float GetJoyconAngle()
    {
        if (Input.GetKey(KeyCode.D)) joyconAngle--;
        if (Input.GetKey(KeyCode.A)) joyconAngle++;
        return joyconAngle;
    }

    public override Vector3 GetJoyconGyro()
    {
        if (Input.GetKey(KeyCode.D)) return new Vector3(0, 0, -5);
        if (Input.GetKey(KeyCode.A)) return new Vector3(0, 0, 5);
        return Vector3.zero;
    }

    public override Vector3 GetJoyconAccel()
    {
        return new Vector3(Input.GetAxis("Horizontal") * 10, 1, 0);
    }
    */
}
