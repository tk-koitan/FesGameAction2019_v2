using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private int value = 1;
    [SerializeField]
    private GameObject star;
    private AudioSource audio;
    private bool isGet;
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 90, 0);
        transform.DORotate(new Vector3(0, -180, 0), 1).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isGet && collision.tag == "Player")
        {
            isGet = true;
            audio.PlayOneShot(audio.clip);
            GameObject tmpObj = Instantiate(star, transform.position, Quaternion.identity);
            Destroy(tmpObj, star.GetComponent<ParticleSystem>().duration);
            transform.DOMoveY(0.5f, 0.5f).SetRelative();
            CoinManager.coinNum++;
            Destroy(gameObject, 0.5f);
        }
    }
}
