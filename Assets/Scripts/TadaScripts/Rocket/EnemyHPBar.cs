using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketStage;
using UnityEngine.UI;

namespace RocketStage
{
    public class EnemyHPBar : MonoBehaviour
    {
        [SerializeField]
        private LastBossController lastBossCtrl;

        // Start is called before the first frame update
        void Start()
        {
            //GetComponent<Image>().fillAmount = 1.0f;
        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<Image>().fillAmount = lastBossCtrl.hp / lastBossCtrl.hpMax;
        }
    }
}