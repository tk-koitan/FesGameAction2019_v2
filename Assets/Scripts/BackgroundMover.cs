using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;

public class BackgroundMover : MonoBehaviour
{
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;
    private float width;
    private float height;
    private SpriteRenderer renderer;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        width = renderer.bounds.size.x;
        height = renderer.bounds.size.y;
        Debug.Log("大きさ:" + renderer.size);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(cam.transform.position.x - cam.orthographicSize*16/9 + width/2, cam.transform.position.y);
        Debug.Log("大きさ:" + renderer.bounds.size);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        GizmosExtensions2D.DrawWireRect2D(new Vector3((minX + maxX) / 2, (minY + maxY) / 2), maxX - minX, maxY - minY);
    }
}
