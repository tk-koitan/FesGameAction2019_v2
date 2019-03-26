using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPuzzleParts : MonoBehaviour
{
    [System.NonSerialized] public bool lightExist = false;
    public bool isPlayer = false;
    public bool useRotation = true;

    public float matchRange = 3f;
    //Transform childTrfm;
    //GameObject lightObjct;

    // childをFindするのと public or [SerializeField] で取得するのどっちが良いんですかね？
    [SerializeField] private Transform childObjectTrfm;
    [SerializeField] private GameObject childLightObject;
     

    Transform playerTrfm;

    // Start is called before the first frame update
    void Start()
    {
        if (!isPlayer)
        {
            //childTrfm = gameObject.transform.Find("Child").transform;
        }
        else
        {
            playerTrfm = GameObject.FindGameObjectWithTag("Player").transform;
        }
        //lightObject = gameObject.transform.Find("Light").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        DeleteLight();
        //Debug.LogFormat("child.x:{0}, my.x:{1}", childTrfm.eulerAngles.z, transform.eulerAngles.z);
        if (!isPlayer)
        {
            if (useRotation)
            {
                if (Mathf.Abs(transform.eulerAngles.z % 180f - (childObjectTrfm.eulerAngles.z) % 180f) < matchRange)
                    SetLight();
                else DeleteLight();
            }
            else // 距離を使う
            {
                if (Vector2.Distance(transform.position, childObjectTrfm.position) < matchRange) SetLight();
                else DeleteLight();
            }
        }
        else
        { // スケール.xも合わせる(向いている方向)
            if (Mathf.Sign(transform.localScale.x) == Mathf.Sign(playerTrfm.localScale.x))
            {
                if (Vector2.Distance(transform.position, playerTrfm.position) < matchRange) SetLight();
                else DeleteLight();
            }
            else DeleteLight();
        }
    }

    private void SetLight()
    {
        if (!lightExist)
        {
            childLightObject.SetActive(true);
            lightExist = true;
        }
    }

    private void DeleteLight()
    {
        if (lightExist)
        {
            childLightObject.SetActive(false);
            lightExist = false;
        }
    }
}
