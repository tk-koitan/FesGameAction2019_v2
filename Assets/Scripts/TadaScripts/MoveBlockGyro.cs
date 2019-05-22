using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;

public class MoveBlockGyro : Mover
{

    public float moveAngle = 0f;
    public float moveDistanceMax = 5.0f; // 中心からどれだけ進めるか
    public float moveSpeed = 0.05f;
    private float moveDistance = 0.0f;
    private AudioSource audioSource;
    private Renderer targetRenderer;

    private ActionInput actionInput;

    protected override void Start()
    {
        actionInput = ActionInput.Instatnce;
        audioSource = GetComponent<AudioSource>();
        targetRenderer = GetComponent<Renderer>();
        base.Start();
    }

    protected override void Update()
    {
        float gyroZ = ActionInput.GetJoyconGyro().z * moveSpeed;

        foreach (Transform child in transform)
        {
            // 子要素の歯車を回転させる
            child.transform.Rotate(0f, 0f, -gyroZ * 20f);
        }

        if (Mathf.Abs(moveDistance + gyroZ) > moveDistanceMax)
            gyroZ = (gyroZ > 0) ? moveDistanceMax - moveDistance : -(moveDistanceMax + moveDistance);

        moveDistance += gyroZ;
        transform.position += new Vector3(gyroZ * Mathf.Cos(moveAngle), gyroZ * Mathf.Sin(moveAngle));

        base.Update();
        // 画面外にいるときは処理を行わないようにしたい 出来れば基底クラスで

        //画面内にいる時に音出す
        if(targetRenderer.isVisible && ActionInput.GetJoyconGyro().z != 0)
        {
            if (!audioSource.isPlaying) audioSource.Play();
            //フェードイン
            audioSource.volume += (1 - audioSource.volume) * 0.2f;
            //Debug.Log("歯車音:" + gyroZ);
        }
        else
        {
            //audioSource.Stop();
            //フェードアウト
            audioSource.volume *=0.9f;
            if (audioSource.volume < 0.01f) audioSource.Stop();
        }
    }

    // Update is called once per frame
    /*
    protected override void Update()
    {
        base.Update();
    }
    */

    private void OnDrawGizmos()
    {
        GizmosExtensions2D.DrawArrow2D(transform.position,
            transform.position + new Vector3((moveDistanceMax - moveDistance) * Mathf.Cos(moveAngle), (moveDistanceMax - moveDistance) * Mathf.Sin(moveAngle)));
        GizmosExtensions2D.DrawArrow2D(transform.position,
            transform.position + new Vector3((-moveDistance - moveDistanceMax) * Mathf.Cos(moveAngle), (-moveDistance - moveDistanceMax) * Mathf.Sin(moveAngle)));
    }
}
