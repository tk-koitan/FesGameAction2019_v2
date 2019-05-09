using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackOtama : MonoBehaviour
{
    public float initSpeed = 1.0f;

    private float height = 8.12f;
    private float width = 18.67f;

    Transform parentTrfm;

    // Start is called before the first frame update
    void Start()
    {
        parentTrfm = transform.root;
    }

    // Update is called once per frame
    void Update()
    {
        //float vx = 
        /*
        float posX = transform.localPosition.x + -speed * Time.deltaTime;
        posX = (posX + height * 16.0f / 9.0f) % (height * 16.0f / 9.0f);
        float posY = transform.localPosition.y + speed * Time.deltaTime;
        posY = (posY + height) % (height);

        transform.localPosition = new Vector2(posX, posY);
        */

        float speed = ActionInput.GetJoyconGyro().z * initSpeed;

        float posX = transform.position.x - parentTrfm.position.x + -speed * 1.2f * Time.deltaTime;
        posX = (posX + width) % (width);
        float posY = transform.position.y - parentTrfm.position.y + speed * Time.deltaTime;
        posY = (posY + height) % (height);

        transform.position = new Vector2(posX + parentTrfm.position.x, posY + parentTrfm.position.y);
    }
}
