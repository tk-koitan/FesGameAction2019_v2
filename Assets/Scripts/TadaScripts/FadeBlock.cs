using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FadeBlock : MonoBehaviour
{

    Tilemap tileMap;

    public float feedTime = 0.4f;
    private bool isFeedOut = false;
    private bool isFeedIn = false;
    private float alpha = 1.0f;

    private void Start()
    {
        tileMap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Debug.Log("enter");
            isFeedOut = true;
            isFeedIn = false;
            StartCoroutine(FeedOut(feedTime));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Debug.Log("exit");
            isFeedIn = true;
            isFeedOut = false;
            StartCoroutine(FeedIn(feedTime));
        }
    }

    private IEnumerator FeedOut(float time)
    {
        while(alpha > 0f && isFeedOut)
        {
            yield return new WaitForSeconds(time / 5f);
            alpha = Mathf.Max(alpha - 1f / (time * 5f), 0f);
            tileMap.color = new Color(tileMap.color.r, tileMap.color.g, tileMap.color.b, alpha);
        }
        isFeedOut = false;
    }
    private IEnumerator FeedIn(float time)
    {
        while (alpha < 1f && isFeedIn)
        {
            yield return new WaitForSeconds(time / 5f);
            alpha = Mathf.Min(alpha + 1f / (time * 5f), 1f);
            tileMap.color = new Color(tileMap.color.r, tileMap.color.g, tileMap.color.b, alpha);
        }
        isFeedIn = false;
    }
}
