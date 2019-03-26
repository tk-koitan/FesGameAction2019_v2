using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsActionEnabled : MonoBehaviour
{
    // Mover忘れてました！！！！！
    // これも割と汎用性あると思うからMoverとのマージわんちゃんないですか！？


    // 派生クラスでのアクションを行えるかどうか
    // 基底クラスを作らずに、[RequireComponent(typeof(IsActionEnabled))]にした方が良いのかも
    /*[System.NonSerialized]*/ public bool actionEnabled = false;

    // isTriggerがtrueだと、一度でもactionEnabledがtrueになったらずっと実行する
    [SerializeField] private bool isTrigger = false;
    private bool reallyEnabled = false;

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        if (GetIFActionEnabled()) UpdatedAction();
    }

    protected virtual void UpdatedAction()
    {
        // 派生クラスで実装
        // この処理は画面外で起こしたくない
        // 取りあえず、プレイヤーもこのギミックの始動要員として画面内のみで可能な処理にしてる
    }

    protected bool GetIFActionEnabled()
    {
        if (!actionEnabled && !isTrigger) return false;

        if (!reallyEnabled || !isTrigger)
            reallyEnabled = actionEnabled;

        return reallyEnabled;
    }
}
