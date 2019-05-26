using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageSelect;
using Cinemachine;
using DG.Tweening;
using UnityEngine.SceneManagement;

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

        // Start is called before the first frame update
        void OnEnable()
        {
            StartCoroutine(RepairAnimation());
        }

        private IEnumerator RepairAnimation()
        {
            vcam.gameObject.SetActive(true);

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

            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("ShootingScene");
        }
    }
}