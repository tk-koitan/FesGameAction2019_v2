using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StaffRoll;

namespace StaffRoll
{
    public class NextScene : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (ActionInput.GetButtonDown(ButtonCode.Jump))
            {
                if(!FadeManager.Instance.isFading)
                    FadeManager.Instance.LoadScene("Title", 2.0f);
            }
        }
    }
}