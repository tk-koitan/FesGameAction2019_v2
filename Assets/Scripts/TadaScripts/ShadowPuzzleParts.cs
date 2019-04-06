using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteGlow;
using DG.Tweening;

public class ShadowPuzzleParts : MonoBehaviour
{
    public bool LightExist { get; private set;}
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
                if (Vector2.Distance(transform.position, playerTrfm.position) < matchRange && 
                    !playerTrfm.GetComponent<PlayerRB>().isSquat) SetLight();
                else DeleteLight();
            }
            else DeleteLight();
        }
    }

    private void SetLight()
    {
        if (!LightExist)
        {
            LightExist = true;

            if (isPlayer)
            {
                playerTrfm.GetComponent<SpriteGlowEffect>().DOKill();
                //playerTrfm.GetComponent<SpriteGlowEffect>().DrawOutside = false;
                DOTween.To(
                    () => playerTrfm.GetComponent<SpriteGlowEffect>().OutlineWidth = 0,
                    num => playerTrfm.GetComponent<SpriteGlowEffect>().OutlineWidth = num,
                    5,
                    1.0f
                    ).SetLoops(-1, LoopType.Yoyo);
                return;
            }

            childObjectTrfm.GetComponent<SpriteGlowEffect>().DOKill();
            childObjectTrfm.GetComponent<SpriteGlowEffect>().DrawOutside = false;
            DOTween.To(
                () => childObjectTrfm.GetComponent<SpriteGlowEffect>().GlowBrightness = 2f,
                num => childObjectTrfm.GetComponent<SpriteGlowEffect>().GlowBrightness = num,
                5f,
                1.0f
                ).SetLoops(-1, LoopType.Yoyo);
            // childLightObject.SetActive(true);
        }
    }

    private void DeleteLight()
    {
        if (LightExist)
        {
            LightExist = false;

            if (isPlayer)
            {
                // うまくDOKillで消せないバグが起きているので後で修正
                playerTrfm.GetComponent<SpriteGlowEffect>().DOKill();
                DOTween.To(
                    () => playerTrfm.GetComponent<SpriteGlowEffect>().OutlineWidth,
                    num => playerTrfm.GetComponent<SpriteGlowEffect>().OutlineWidth = num,
                    0,
                    0.2f
                    );//.OnComplete(() => playerTrfm.GetComponent<SpriteGlowEffect>().DrawOutside = true);
                return;
            }

            childObjectTrfm.GetComponent<SpriteGlowEffect>().DOKill();
            DOTween.To(
                () => childObjectTrfm.GetComponent<SpriteGlowEffect>().GlowBrightness,
                num => childObjectTrfm.GetComponent<SpriteGlowEffect>().GlowBrightness = num,
                2f,
                0.2f
                ).OnComplete(() => childObjectTrfm.GetComponent<SpriteGlowEffect>().DrawOutside = true);
            //childObjectTrfm.GetComponent<SpriteGlowEffect>().DrawOutside = true;
            //childLightObject.SetActive(false);
        }
    }
}
