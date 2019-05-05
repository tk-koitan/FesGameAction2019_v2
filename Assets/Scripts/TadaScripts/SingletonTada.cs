using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonTada : MonoBehaviour
{
    static SingletonTada instance;
    public static SingletonTada Instatnce
    {
        get { return instance; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }
}
