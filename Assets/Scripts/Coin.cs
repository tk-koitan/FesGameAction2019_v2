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
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 90, 0);
        transform.DORotate(new Vector3(0, -180, 0), 1).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameObject tmpObj = Instantiate(star, transform.position, Quaternion.identity);
            Destroy(tmpObj, star.GetComponent<ParticleSystem>().duration);
            transform.DOMoveY(0.5f, 0.5f).SetRelative();
            Destroy(gameObject,0.5f);

        }
    }
}
