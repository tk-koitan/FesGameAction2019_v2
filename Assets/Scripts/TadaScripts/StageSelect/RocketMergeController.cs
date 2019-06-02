using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageSelect;
using Cinemachine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StageSelect
{
    public class RocketMergeController : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem mergingEffect;
        [SerializeField]
        private ParticleSystem repairedEffect;
        [SerializeField]
        private Transform allRocketTrfm;

        [SerializeField]
        private float mergingDuration = 3.0f;
        [SerializeField]
        private float raiseRocketY = 2.0f;
        [SerializeField]
        private CinemachineVirtualCamera vcam;

        [SerializeField]
        private Image stageNameFlame;

        private Camera cam;

        // Start is called before the first frame update
        void OnEnable()
        {
            cam = Camera.main;
            StartCoroutine(RepairAnimation());
        }

        private IEnumerator RepairAnimation()
        {
            // default , UIを非表示にする
           // cam.cullingMask &= ~(1 << LayerMask.NameToLayer("UI"));
            //cam.cullingMask &= ~(1 << LayerMask.NameToLayer("Default"));

            // カメラを変更
            vcam.gameObject.SetActive(true);
            // ステージ名を出す枠を非表示に
            stageNameFlame.gameObject.SetActive(false);

            mergingEffect.Play();
            transform.DOMoveY(transform.position.y + raiseRocketY, 1.0f).SetEase(Ease.OutQuart);
            allRocketTrfm.DOMoveY(allRocketTrfm.position.y + raiseRocketY, 1.0f).SetEase(Ease.OutQuart);

            yield return new WaitForSeconds(mergingDuration);

            mergingEffect.Stop();

            foreach(Transform child in allRocketTrfm)
            {
                child.gameObject.SetActive(false);
            }

            repairedEffect.Play();
            GetComponent<SpriteRenderer>().enabled = true;

            ActionInput.actionEnabled = true;

            vcam.gameObject.SetActive(false);
            stageNameFlame.gameObject.SetActive(true);

            // UI,defaultを表示する
            //cam.cullingMask |= 1 << LayerMask.NameToLayer("UI");
            //cam.cullingMask |= 1 << LayerMask.NameToLayer("Default");

            //yield return new WaitForSeconds(3.0f);
            //SceneManager.LoadScene("ShootingScene");
        }
    }
}