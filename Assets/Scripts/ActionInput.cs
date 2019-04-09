using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ActionInput : MonoBehaviour
{
    public static bool actionEnabled;
    //public abstract bool GetButtonDown(ButtonCode code);
    //public abstract bool GetButton(ButtonCode code);
    //public abstract bool GetButtonUp(ButtonCode code);

    //public abstract float GetJoyconAngle();
    //public abstract Vector3 GetJoyconGyro();
    //public abstract Vector3 GetJoyconAccel();

    public delegate bool Button(ButtonCode code);
    public delegate float Axis(AxisCode code);
    public delegate Vector3 JoyconSensor();
    public static Button GetButtonDown, GetButton, GetButtonUp;
    public static Axis GetAxis;
    public static JoyconSensor GetJoyconVector, GetJoyconGyro, GetJoyconAccel;
    private static float joyconAngle = 0;
    private Joycon stickJoycon;
    private Joycon gyroJoycon;

    static ActionInput instance;
    public static ActionInput Instatnce
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        SetControllerKeyboard();
        SetSensorKeyboard();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        if (stickJoycon == null)
        {
            var joycons = JoyconManager.Instance.j;
            stickJoycon = joycons.Find(c => c.isLeft);
            if (stickJoycon == null)
            {
                stickJoycon = joycons.Find(c => !c.isLeft);
                SetControllerJoycon(stickJoycon);
            }
        }

        if (gyroJoycon == null)
        {
            var joycons = JoyconManager.Instance.j;
            gyroJoycon = joycons.Find(c => !c.isLeft);
            if (gyroJoycon == null)
            {
                gyroJoycon = joycons.Find(c => c.isLeft);
                SetSensorJoycon(gyroJoycon);
            }
        }

        foreach (Joycon joycon in JoyconManager.Instance.j)
        {
            if (joycon.GetButton(Joycon.Button.SL) && joycon.GetButton(Joycon.Button.SR))
            {
                stickJoycon = joycon;
                SetControllerJoycon(stickJoycon);
            }

            if (joycon.GetButton(Joycon.Button.SHOULDER_1) && joycon.GetButton(Joycon.Button.SHOULDER_2))
            {
                gyroJoycon = joycon;
                SetSensorJoycon(gyroJoycon);
            }
        }

        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            SetControllerKeyboard();
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            SetSensorKeyboard();
        }

        //ジョイコンの補正
        if(gyroJoycon!=null)
        {
            if (gyroJoycon.GetButtonDown(Joycon.Button.SHOULDER_2))
            {
                gyroJoycon.Recenter();
            }
        }

    }

    public static void SetControllerKeyboard()
    {
        GetButtonDown = (code) =>
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
        };

        GetButton = (code) =>
        {
            if (!actionEnabled) return false;
            switch (code)
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
        };

        GetButtonUp = (code) =>
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
        };

        GetAxis = (code) =>
        {
            if (!actionEnabled) return 0;
            switch (code)
            {
                case AxisCode.Horizontal:
                    return Input.GetAxis("Horizontal");
                case AxisCode.Vertical:
                    return Input.GetAxis("Vertical");
            }
            return 0;
        };
    }

    public static void SetSensorKeyboard()
    {
        GetJoyconVector = () =>
        {
            if (Input.GetKey(KeyCode.D)) joyconAngle--;
            if (Input.GetKey(KeyCode.A)) joyconAngle++;
            return new Vector3(0, joyconAngle, 0);
        };

        GetJoyconGyro = () =>
        {
            if (Input.GetKey(KeyCode.D)) return new Vector3(0, 0, -5);
            if (Input.GetKey(KeyCode.A)) return new Vector3(0, 0, 5);
            return Vector3.zero;
        };

        GetJoyconAccel = () =>
        {
            return new Vector3(Input.GetAxis("Horizontal") * 10, 1, 0);
        };
    }

    public static void SetControllerJoycon(Joycon joycon)
    {
        if (joycon.isLeft)
        {
            GetButtonDown = (code) =>
            {
                if (!actionEnabled) return false;
                switch (code)
                {
                    case ButtonCode.Jump:
                        return joycon.GetButtonDown(Joycon.Button.DPAD_LEFT);
                    case ButtonCode.UpArrow:
                        return joycon.GetStick()[0] > 0;
                    case ButtonCode.DownArrow:
                        return joycon.GetStick()[0] < 0;
                    case ButtonCode.LeftArrow:
                        return joycon.GetStick()[1] > 0;
                    case ButtonCode.RightArrow:
                        return joycon.GetStick()[1] < 0;
                }
                return false;
            };

            GetButton = (code) =>
            {
                if (!actionEnabled) return false;
                switch (code)
                {
                    case ButtonCode.Jump:
                        return joycon.GetButton(Joycon.Button.DPAD_LEFT);
                    case ButtonCode.UpArrow:
                        return joycon.GetStick()[0] > 0;
                    case ButtonCode.DownArrow:
                        return joycon.GetStick()[0] < 0;
                    case ButtonCode.LeftArrow:
                        return joycon.GetStick()[1] > 0;
                    case ButtonCode.RightArrow:
                        return joycon.GetStick()[1] < 0;
                }
                return false;
            };

            GetButtonUp = (code) =>
            {
                if (!actionEnabled) return false;
                switch (code)
                {
                    case ButtonCode.Jump:
                        return joycon.GetButtonUp(Joycon.Button.DPAD_LEFT);
                    case ButtonCode.UpArrow:
                        return joycon.GetStick()[0] > 0;
                    case ButtonCode.DownArrow:
                        return joycon.GetStick()[0] < 0;
                    case ButtonCode.LeftArrow:
                        return joycon.GetStick()[1] > 0;
                    case ButtonCode.RightArrow:
                        return joycon.GetStick()[1] < 0;
                }
                return false;
            };
        }
        else
        {
            GetButtonDown = (code) =>
            {
                if (!actionEnabled) return false;
                switch (code)
                {
                    case ButtonCode.Jump:
                        return joycon.GetButtonDown(Joycon.Button.DPAD_RIGHT);
                    case ButtonCode.UpArrow:
                        return joycon.GetStick()[0] < 0;
                    case ButtonCode.DownArrow:
                        return joycon.GetStick()[0] > 0;
                    case ButtonCode.LeftArrow:
                        return joycon.GetStick()[1] < 0;
                    case ButtonCode.RightArrow:
                        return joycon.GetStick()[1] > 0;
                }
                return false;
            };

            GetButton = (code) =>
            {
                if (!actionEnabled) return false;
                switch (code)
                {
                    case ButtonCode.Jump:
                        return joycon.GetButton(Joycon.Button.DPAD_RIGHT);
                    case ButtonCode.UpArrow:
                        return joycon.GetStick()[0] < 0;
                    case ButtonCode.DownArrow:
                        return joycon.GetStick()[0] > 0;
                    case ButtonCode.LeftArrow:
                        return joycon.GetStick()[1] < 0;
                    case ButtonCode.RightArrow:
                        return joycon.GetStick()[1] > 0;
                }
                return false;
            };

            GetButtonUp = (code) =>
            {
                if (!actionEnabled) return false;
                switch (code)
                {
                    case ButtonCode.Jump:
                        return joycon.GetButtonUp(Joycon.Button.DPAD_RIGHT);
                    case ButtonCode.UpArrow:
                        return joycon.GetStick()[0] < 0;
                    case ButtonCode.DownArrow:
                        return joycon.GetStick()[0] > 0;
                    case ButtonCode.LeftArrow:
                        return joycon.GetStick()[1] < 0;
                    case ButtonCode.RightArrow:
                        return joycon.GetStick()[1] > 0;
                }
                return false;
            };
        }
    }

    public static void SetSensorJoycon(Joycon joycon)
    {
        GetJoyconVector = () =>
        {
            return joycon.GetVector().eulerAngles;
        };

        GetJoyconGyro = joycon.GetGyro;

        GetJoyconAccel = joycon.GetAccel;
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

public enum AxisCode
{
    Horizontal,
    Vertical
}
