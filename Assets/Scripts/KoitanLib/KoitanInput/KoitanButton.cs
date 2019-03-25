using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoitanButton
{

    private string name;
    private ConType conType;
    private int axisNum;
    private float deadline;
    private bool isInvert;
    private int buttonNum;
    private int orderNum;
    private KeyCode keyCode;

    private bool fdValue = false;
    private bool cdValue = false;
    private float dNow = 0;

    private bool fuValue = false;
    private bool cuValue = false;
    private float uNow = 0;

    private bool isAI;
    private bool aiValue;


    public KoitanButton(ConType conType, int orderNum, int axisNum, bool isInvert, float deadline)
    {
        this.conType = conType;
        switch (conType)
        {
            case ConType.JoyAxis:
                this.orderNum = orderNum;
                this.axisNum = axisNum;
                this.isInvert = isInvert;
                this.deadline = deadline;
                this.name = "joystick " + orderNum.ToString() + " analog " + axisNum.ToString();
                break;
        }
    }

    public KoitanButton(ConType conType, int orderNum, int buttonNum)
    {
        this.conType = conType;
        switch (conType)
        {
            case ConType.JoyButton:
                this.orderNum = orderNum;
                this.buttonNum = buttonNum;
                this.name = "joystick " + orderNum.ToString() + " button " + buttonNum.ToString();
                break;
        }
    }

    public KoitanButton(ConType conType, KeyCode keyCode)
    {
        this.conType = conType;
        switch (conType)
        {
            case ConType.Key:
                this.keyCode = keyCode;
                break;
        }
    }

    public void SetIsAI(bool avilable)
    {
        isAI = avilable;
    }

    public void SetAIValue(bool value)
    {
        aiValue = value;
    }

    public bool GetButton()
    {
        //外部からの操作を受け付ける
        if (isAI) return aiValue;
        switch (conType)
        {
            case ConType.JoyAxis:
                float inputValue = Input.GetAxis(name);
                if ((isInvert ? -inputValue : inputValue) < deadline) return false;
                else return true;

            case ConType.JoyButton:
                return Input.GetKey(name);

            case ConType.Key:
                return Input.GetKey(keyCode);
        }
        return false;
    }

    public bool GetButtonDown()
    {
        if (dNow != Time.time)
        {
            dNow = Time.time;
            cdValue = GetButton();
            if (fdValue == false && cdValue == true)
            {
            }
            else
            {
                cdValue = false;
            }
            fdValue = GetButton();
        }
        return cdValue;
    }

    public bool GetButtonUp()
    {
        if (uNow != Time.time)
        {
            uNow = Time.time;
            cuValue = GetButton();
            if ((fuValue == true && cuValue == false))
            {
                cuValue = fuValue;
            }
            else
            {
                cuValue = false;
            }
            fuValue = GetButton();
        }
        return cuValue;
    }

}
