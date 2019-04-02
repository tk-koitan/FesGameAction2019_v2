using UnityEngine;

public class RuntimeInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeBeforeSceneLoad()
    {
        // ゲーム中に常に存在するオブジェクトを生成、およびシーンの変更時にも破棄されないようにする。
        var manager = new GameObject("JoyconManager", typeof(JoyconManager));
        manager.AddComponent<Example>();
        //Debug.Log("シーン読み込み前JoyconManagerInstatnce:" + JoyconManager.Instance.j.Count);
        if(JoyconManager.Instance.j.Count>0)
        {
            manager.AddComponent<JoyConInput>();
        }
        else
        {
            manager.AddComponent<PlayerInput>();
        }
        GameObject.DontDestroyOnLoad(manager);

        //操作可能
        ActionInput.actionEnabled = true;
    }

} // class RuntimeInitializer