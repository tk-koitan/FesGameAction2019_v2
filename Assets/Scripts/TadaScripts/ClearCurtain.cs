using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClearCurtain : MonoBehaviour
{
    public float animTime = 3.0f;

    private bool isDone = false;
    [SerializeField] private GameObject clearCurtain;

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDone) return;

        if(collision.tag == "Player")
        {
            // ステージのクリアをtrueにする

            StageSelect.StageTable.stageClearedList[StageSelect.StageTable.challengeStageId] = true;

            isDone = true;
            GetComponent<Animator>().SetTrigger("Clear");
            collision.GetComponent<PlayerRB>().Goaled();
            Invoke("EndCurtain", 2.0f);
        }
    }

    private void EndCurtain()
    {
        // 幕をカメラの位置にする
        clearCurtain.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);

        DOTween.To(
            () => clearCurtain.GetComponent<SpriteMask>().alphaCutoff,
            num => clearCurtain.GetComponent<SpriteMask>().alphaCutoff = num,
            1f,
            animTime
            );
    }
}
