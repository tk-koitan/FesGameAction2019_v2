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
        // 子要素がすべて光っていたらギミック始動
        /*foreach(Transform childTransform in transform)
        {
            if (childTransform.GetComponent<MatchPuzzle>().lightExist == false)
                return;
        }*/

        actionEnabledPrev = actionEnabled;
        actionEnabled = true;

        foreach(GameObject shadowObj in shadowObjects)
        {
            if (shadowObj.GetComponent<ShadowPuzzleParts>().lightExist == false)
            {
                actionEnabled = false;
                break;
            }
        }

        if (actionEnabled != actionEnabledPrev)
        {
            foreach (GameObject actionObj in actionObjects)
            {
                actionObj.GetComponent<Mover>().actionEnabled = actionEnabled;
            }
        }

    }
}
