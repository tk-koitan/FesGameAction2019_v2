using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleJoyconRotator : Mover
{
    private float startAngle;

    protected override void Start()
    {
        base.Start();
        startAngle = transform.localEulerAngles.z;
    }

    protected override void Update()
    {
        base.Update();

        transform.rotation = Quaternion.Euler(new Vector3(0, 0,ActionInput.GetJoyconVector().y + startAngle));
        transform.localScale = ActionInput.GetJoyconAccel();
        Vector3 tmpS = transform.localScale;
        /*
        tmpS.x = Mathf.Max(tmpS.x, 0.5f);
        tmpS.y = Mathf.Max(tmpS.y, 0.5f);
        tmpS.z = Mathf.Max(tmpS.z, 0.5f);
        transform.localScale = tmpS;
        */       
    }
}