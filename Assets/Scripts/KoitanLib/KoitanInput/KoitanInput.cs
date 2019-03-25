using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KoitanLib
{
    public class KoitanInput : MonoBehaviour
    {
        static KoitanInput instance;
        public static KoitanInput Instance
        {
            get { return instance; }
        }

        void Awake()
        {
            if (instance != null) Destroy(gameObject);
            instance = null;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}

public enum Axis
{
    L_Horizontal,
    L_Vertical,
    R_Horizontal,
    R_Vertical,
    Cross_Horizontal,
    Cross_Vertical
}

public enum ButtonName
{
    Jump,
    Attack,
    SubAttack
}

public enum AxisName
{
    MoveX,
    MoveY
}
public enum ConType
{
    JoyButton,
    JoyAxis,
    Key,
    MouseMovement
}

