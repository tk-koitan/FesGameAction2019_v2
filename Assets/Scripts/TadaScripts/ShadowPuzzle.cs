using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPuzzle : MonoBehaviour
{
    public GameObject[] shadowObjects;
    public GameObject[] actionObjects;

    private bool actionEnabled = false;
    private bool actionEnabledPrev = false;

    // Update is called once per frame
    void Update()
    {
        actionEnabledPrev = actionEnabled;
        actionEnabled = IsLightAllLamped();

        if (actionEnabled != actionEnabledPrev)
        {
            SetActionEnabled(actionEnabled);
        }
    }

    private bool IsLightAllLamped()
    {
        foreach(GameObject shadowObj in shadowObjects){
            if (shadowObj.GetComponent<ShadowPuzzleParts>().LightExist == false)
                return false;
        }
        return true;
    }

    private void SetActionEnabled(bool isEnabled)
    {
        foreach(GameObject actionObje in actionObjects)
        {
            actionObje.GetComponent<Mover>().actionEnabled = isEnabled;
        }
    }
}
