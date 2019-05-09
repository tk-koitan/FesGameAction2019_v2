using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionInput : MonoBehaviour
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
    private static float joyconAngle = 180; // キー操作の時にデフォルトで上を向くように変えました tada
    private Joycon stickJoycon;
    private Joycon gyroJoycon;
    private static float beforeHorizontalValue;
    private static float beforeVerticalValue;


    static ActionInput instance;
    public static ActionInput Instatnce
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        Debug.Log("コントローラーにキーボードが設定されました");
        SetControllerKeyboard();
        SetSensorKeyboard();

        Debug.Log("ActionInoput.Awake()");
        var joycons = JoyconManager.Instance.j;
        stickJoycon = joycons.Find(c => c.isLeft);
        if (stickJoycon == null)
        {
            stickJoycon = joycons.Find(c => !c.isLeft);
            if (stickJoycon != null)
            {
                Debug.Log("コントローラーにジョイコンが設定されました");
                SetControllerJoycon(stickJoycon);
            }
        }
        else
        {
            SetControllerJoycon(stickJoycon);
        }

        gyroJoycon = joycons.Find(c => !c.isLeft);
        if (gyroJoycon == null)
        {
            gyroJoycon = joycons.Find(c => c.isLeft);
            if (gyroJoycon != null)
            {
                SetSensorJoycon(gyroJoycon);
            }
        }
        else
        {
            SetSensorJoycon(gyroJoycon);
        }

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
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
        if (gyroJoycon != null)
        {
            if (gyroJoycon.GetButtonDown(Joycon.Button.SHOULDER_2))
            {
                gyroJoycon.Recenter();
            }
        }

        //テスト
        /*
        if(ActionInput.GetButtonDown(ButtonCode.UpArrow))
        {
            Debug.Log("上が押されました");
        }
        if (ActionInput.GetButton(ButtonCode.UpArrow))
        {
            Debug.Log("上が押されてます");
        }
        if (ActionInput.GetButtonUp(ButtonCode.UpArrow))
        {
            Debug.Log("上が離されました");
        }
        */
    }

    protected virtual void LateUpdate()
    {
        if (stickJoycon != null)
        {
            beforeHorizontalValue = stickJoycon.GetStick()[1];
            beforeVerticalValue = stickJoycon.GetStick()[0];
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
                case ButtonCode.Cancel:
                    return Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete);
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
                case ButtonCode.Cancel:
                    return Input.GetKey(KeyCode.Backspace) || Input.GetKey(KeyCode.Delete);
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
                case ButtonCode.Cancel:
                    return Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Delete);
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
                        if (beforeVerticalValue <= 0)
                        {
                            return joycon.GetStick()[0] > 0;
                        }
                        return false;
                    case ButtonCode.DownArrow:
                        if (beforeVerticalValue >= 0)
                        {
                            return joycon.GetStick()[0] < 0;
                        }
                        return false;
                    case ButtonCode.LeftArrow:
                        if (beforeHorizontalValue <= 0)
                        {
                            return joycon.GetStick()[1] > 0;
                        }
                        return false;
                    case ButtonCode.RightArrow:
                        if (beforeHorizontalValue >= 0)
                        {
                            return joycon.GetStick()[1] < 0;
                        }
                        return false;
                    case ButtonCode.Cancel:
                        return joycon.GetButtonDown(Joycon.Button.DPAD_DOWN);
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
                    case ButtonCode.Cancel:
                        return joycon.GetButton(Joycon.Button.DPAD_DOWN);
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
                        if (beforeVerticalValue > 0)
                        {
                            return joycon.GetStick()[0] <= 0;
                        }
                        return false;
                    case ButtonCode.DownArrow:
                        if (beforeVerticalValue < 0)
                        {
                            return joycon.GetStick()[0] >= 0;
                        }
                        return false;
                    case ButtonCode.LeftArrow:
                        if (beforeHorizontalValue > 0)
                        {
                            return joycon.GetStick()[1] <= 0;
                        }
                        return false;
                    case ButtonCode.RightArrow:
                        if (beforeHorizontalValue < 0)
                        {
                            return joycon.GetStick()[1] >= 0;
                        }
                        return false;
                    case ButtonCode.Cancel:
                        return joycon.GetButtonUp(Joycon.Button.DPAD_DOWN);
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
                        if (beforeVerticalValue >= 0)
                        {
                            return joycon.GetStick()[0] < 0;
                        }
                        return false;
                    case ButtonCode.DownArrow:
                        if (beforeVerticalValue <= 0)
                        {
                            return joycon.GetStick()[0] > 0;
                        }
                        return false;
                    case ButtonCode.LeftArrow:
                        if (beforeHorizontalValue >= 0)
                        {
                            return joycon.GetStick()[1] < 0;
                        }
                        return false;
                    case ButtonCode.RightArrow:
                        if (beforeHorizontalValue <= 0)
                        {
                            return joycon.GetStick()[1] > 0;
                        }
                        return false;
                    case ButtonCode.Cancel:
                        return joycon.GetButtonDown(Joycon.Button.DPAD_UP);
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
                    case ButtonCode.Cancel:
                        return joycon.GetButton(Joycon.Button.DPAD_UP);
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
                        if (beforeVerticalValue < 0)
                        {
                            return joycon.GetStick()[0] >= 0;
                        }
                        return false;
                    case ButtonCode.DownArrow:
                        if (beforeVerticalValue > 0)
                        {
                            return joycon.GetStick()[0] <= 0;
                        }
                        return false;
                    case ButtonCode.LeftArrow:
                        if (beforeHorizontalValue < 0)
                        {
                            return joycon.GetStick()[1] >= 0;
                        }
                        return false;
                    case ButtonCode.RightArrow:
                        if (beforeHorizontalValue > 0)
                        {
                            return joycon.GetStick()[1] <= 0;
                        }
                        return false;
                    case ButtonCode.Cancel:
                        return joycon.GetButtonUp(Joycon.Button.DPAD_UP);
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
    Cancel
}

public enum AxisCode
{
    Horizontal,
    Vertical
}
