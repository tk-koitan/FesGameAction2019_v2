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
        width = renderer.bounds.size.x;
        height = renderer.bounds.size.y;
        float tx = Mathf.Clamp01((cam.transform.position.x - minX - cam.orthographicSize * 16 / 9) / (maxX - minX - cam.orthographicSize * 32 / 9));
        //transform.position = new Vector3(cam.transform.position.x - cam.orthographicSize * 16 / 9 + width / 2 - t * (maxX - minX + 2 * cam.orthographicSize * 16 / 9 - 2 * width), cam.transform.position.y);
        float tmpX = cam.transform.position.x - cam.orthographicSize * 16 / 9 + width / 2 + tx * 2 * (cam.orthographicSize * 16 / 9 - width / 2);
        float ty = Mathf.Clamp01((cam.transform.position.y - minY - cam.orthographicSize) / (maxY - minY - cam.orthographicSize * 2));
        float tmpY = cam.transform.position.y - cam.orthographicSize + height / 2 + ty * 2 * (cam.orthographicSize - height / 2);
        transform.position = new Vector3(tmpX, tmpY);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        GizmosExtensions2D.DrawWireRect2D(new Vector3((minX + maxX) / 2, (minY + maxY) / 2), maxX - minX, maxY - minY);
    }
}
