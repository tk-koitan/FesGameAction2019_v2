using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;
using TMPro;

namespace StaffRoll {
    public class StaffRollManager : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI[] tmpro;

        [SerializeField]
        private BaseRocketController rocket;

        [SerializeField]
        private float eachWaitTime = 3.0f;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(RocketFlow());
        }

        private IEnumerator RocketFlow()
        {
            rocket.DoBeginAnimation2();

            yield return new WaitForSeconds(rocket.startAnimationTime / 2f);

            rocket.actionEnabled = true;

            for(int i = 0; i < tmpro.Length; i++)
            {
                if (i >= tmpro.Length - 3)
                     yield return new WaitForSeconds(eachWaitTime * 2.0f);
                else 
                    yield return new WaitForSeconds(eachWaitTime);

                tmpro[i].gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(eachWaitTime * 2.0f);

            FadeManager.Instance.LoadScene("Title", 2.0f);
        }
    }
}