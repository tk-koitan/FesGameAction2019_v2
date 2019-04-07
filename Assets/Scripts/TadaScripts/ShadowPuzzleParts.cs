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

    [SerializeField] private Transform childObjectTrfm;
    [SerializeField] private GameObject childLightObject;
     

    private Transform playerTrfm;

    private Tween lightTween;


    // Start is called before the first frame update
    void Start()
    {
        if (isPlayer)
            playerTrfm = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("LightExist" + LightExist);
        if (!isPlayer)
        {
            if (useRotation)
            {
                if (IsMatchAngle(childObjectTrfm.eulerAngles.z))
                {
                    SetLight();
                    return;
                }
            }
            else // 距離を使う
            {
                if (IsMatchDistance(childObjectTrfm.position))
                {
                    SetLight();
                    return;
                }
            }
        }
        else
        { // スケール.xも合わせる(向いている方向)
            if (IsMatchDirection() && IsMatchDistance(playerTrfm.position))
            {
                if (!playerTrfm.GetComponent<PlayerRB>().isSquat)
                {
                    SetLight();
                    return;
                }
            }
        }

        DeleteLight();
    }

    private void SetLight()
    {
        if (LightExist) return;

        LightExist = true;

        if (lightTween != null)
            lightTween.Kill();

        if (isPlayer)
        {
            lightTween = DOTween.To(
                () => playerTrfm.GetComponent<SpriteGlowEffect>().OutlineWidth = 0,
                num => playerTrfm.GetComponent<SpriteGlowEffect>().OutlineWidth = num,
                4,
                1.0f
                ).SetLoops(-1, LoopType.Yoyo);
            return;
        }

        childObjectTrfm.GetComponent<SpriteGlowEffect>().DrawOutside = false;
        lightTween = DOTween.To(
            () => childObjectTrfm.GetComponent<SpriteGlowEffect>().GlowBrightness = 2f,
            num => childObjectTrfm.GetComponent<SpriteGlowEffect>().GlowBrightness = num,
            5f,
            1.0f
            ).SetLoops(-1, LoopType.Yoyo);
        // childLightObject.SetActive(true);
    }

    private void DeleteLight()
    {

        if (!LightExist) return;

        LightExist = false;

        if (lightTween != null)
            lightTween.Kill();

        if (isPlayer)
        {
            lightTween = DOTween.To(
                () => playerTrfm.GetComponent<SpriteGlowEffect>().OutlineWidth,
                num => playerTrfm.GetComponent<SpriteGlowEffect>().OutlineWidth = num,
                0,
                0.2f
                );
            return;
        }

        lightTween = DOTween.To(
            () => childObjectTrfm.GetComponent<SpriteGlowEffect>().GlowBrightness,
            num => childObjectTrfm.GetComponent<SpriteGlowEffect>().GlowBrightness = num,
            2f,
            0.2f
            ).OnComplete(() => childObjectTrfm.GetComponent<SpriteGlowEffect>().DrawOutside = true);
        //childLightObject.SetActive(false);
    }

    private bool IsMatchAngle(float targetAngleZ)
    {
        return Mathf.Abs(transform.eulerAngles.z % 180f - (targetAngleZ) % 180f) < matchRange;
    }

    private bool IsMatchDistance(Vector3 targetPos)
    {
        return Vector2.Distance(transform.position, targetPos) < matchRange;
    }

    private bool IsMatchDirection()
    {
        return Mathf.Sign(transform.localScale.x) == Mathf.Sign(playerTrfm.localScale.x);
    }
}
