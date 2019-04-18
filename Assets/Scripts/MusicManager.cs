using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    static MusicManager instance;
    public static MusicManager Instance
    {
        get { return instance; }
    }

    public static AudioSource audioSource;
    public BGMSource bgm1, bgm2, bgm3;
    public float startTime, endTime;
    private float currentTime;
    public bool isIntro = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(audioSource.time>=startTime)
        {
            isIntro = false;
        }
        if (audioSource.time >= endTime)
        {
            audioSource.time = startTime;
            audioSource.Play();
        }
        if(!isIntro && audioSource.time<=0)
        {
            audioSource.time = startTime;
            audioSource.Play();
        }
        Debug.Log(audioSource.time);
    }

    public static void Play(BGMSource bgm)
    {
        instance.startTime = bgm.startTime;
        instance.endTime = bgm.endTime;
        audioSource.clip = bgm.BGM;
        audioSource.time = 0;
        audioSource.Play();
        instance.isIntro = true;
    }
}
