using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageSelect;
using Cinemachine;
using DG.Tweening;
using KoitanLib;

namespace StageSelect
{
    /// <summary>
    /// ステージをクリアしロケットの破片を手に入れた場合に
    /// その破片をマップ中央に送る演出を制御する
    /// </summary>
    public class StageCleared : MonoBehaviour
    {
        [SerializeField]
        private int stageId = 0;

        [SerializeField]
        private CinemachineVirtualCamera vcamera;

        [SerializeField]
        private Vector3[] path;
        [SerializeField]
        private Transform destination;
        [SerializeField]
        private float moveTime = 5.0f;

        [SerializeField]
        private ParticleSystem getEffect;
        [SerializeField]
        private ParticleSystem setEffect;

        // Start is called before the first frame update
        void Start()
        {
            // 元からクリア済みならリターン
            if (StageTable.prevStageClearedList[stageId])
            {
                GetComponent<SpriteRenderer>().enabled = true;
                transform.position = destination.position;
                return;
            }

            bool isStageCleared = StageTable.stageClearedList[stageId];
            // 始めてクリアしたときだけ演出
            if(isStageCleared && !StageTable.prevStageClearedList[stageId])
            {
                StartCoroutine(GetFlagmentAnimation());
            }
            StageTable.prevStageClearedList[stageId] = isStageCleared;
        }

        private IEnumerator GetFlagmentAnimation()
        {
            // スプライトを表示する
            GetComponent<SpriteRenderer>().enabled = true;

            // 操作を受け付けなくする
            ActionInput.actionEnabled = false;

            // カメラを有効にする
            vcamera.gameObject.SetActive(true);
            vcamera.Follow = gameObject.transform;

            // pathに最後の目的地を含める
            path[path.Length - 1] = destination.position;

            feedObj(GetComponent<SpriteRenderer>(), 1.0f, 0f, 1f);

            /*transform.DOLocalRotate(
                new Vector3(0f, 0f, 1080f),
                3.0f,
                RotateMode.FastBeyond360);
                */
            yield return new WaitForSeconds(0.5f);

            PlayEffect(getEffect);

            // 移動する
            transform.DOLocalPath(
                path,
                moveTime,
                PathType.CatmullRom).SetEase(Ease.InOutQuart).OnComplete(() => EndAnimation());
        }

        private void EndAnimation()
        {
            PlayEffect(setEffect);
            ActionInput.actionEnabled = true;
            vcamera.gameObject.SetActive(false);
        }

        private void feedObj(SpriteRenderer target, float time, float firstValue, float endValue)
        {
            DOTween.To(
                () => target.color = new Color(0f, 0f, 0f, firstValue),
                num => target.color = num,
                new Color(target.color.r, target.color.g, target.color.b, endValue),
                time
                ).SetEase(Ease.InOutCubic);
        }

        private void PlayEffect(ParticleSystem effect)
        {
            effect.transform.position = transform.position;
            effect.Play();
        }

        /*
        private void GetFlagmentAnimation()
        {
            // スプライトを表示する
            GetComponent<SpriteRenderer>().enabled = true;

            // 操作を受け付けなくする
            ActionInput.actionEnabled = false;

            // カメラを有効にする
            vcamera.gameObject.SetActive(true);
            vcamera.Follow = gameObject.transform;

            // pathに最後の目的地を含める
            path[path.Length - 1] = destination.position;

            // 移動する
            transform.DOLocalPath(
                path,
                moveTime,
                PathType.CatmullRom).SetEase(Ease.InQuint).OnComplete(() => EndAnimation());
        }*/

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (StageTable.prevStageClearedList[stageId]) return;

            Vector3 from = transform.position;
            Vector3 to;
            foreach (Vector3 path in path)
            {
                to = path;
                GizmosExtensions2D.DrawArrow2D(from, to);
                from = to;
            }
            to = destination.position;
            GizmosExtensions2D.DrawArrow2D(from, to);
        }
#endif
    }
}