using UnityEngine;

public class Example2 : MonoBehaviour
{
    private Joycon m_joyconR;

    private void Start()
    {
        var joycons = JoyconManager.Instance.j;
        m_joyconR = joycons.Find( c => !c.isLeft );
    }

    private void Update()
    {
        var orientation = m_joyconR.GetVector().eulerAngles;
        var angles = transform.localEulerAngles;
        angles.z = orientation.y;
        transform.localEulerAngles = angles;

        if (m_joyconR.GetButtonDown(Joycon.Button.SHOULDER_2))
        {
            m_joyconR.Recenter();
        }

        transform.position = new Vector3(0, m_joyconR.GetAccel().magnitude-1, 0);
    }
}