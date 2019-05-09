using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class RocketController : MonoBehaviour
{
    public float speed = 2.0f;
    public float speedVx { private set; get; }
    public float speedVy { private set; get; }

    private float posY;
    public float upBorder = 3.0f;
    public float headWind = -0.5f;

    public bool isDead = false;

    public Vector2 border = new Vector2(4.0f, 2.0f);

    public bool actionEnabled { private set; get; } 
    public float startAnimationTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        actionEnabled = false;
        //posY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        SetRotation();
        SetVelocity();

        posY = Mathf.Min(posY + speedVy * Time.deltaTime, upBorder);

        //Debug.Log(speedVx + " " + speedVy);
        float posX = Mathf.Clamp(transform.position.x + speedVx * Time.deltaTime, -border.x, border.x);

        if (actionEnabled)
            transform.position = new Vector2(posX, posY);
        IsBorderInner();

        if (isDead)
            GoNextScene();
    }

    private void SetRotation()
    {
        Vector3 orientation = ActionInput.GetJoyconVector();
        Vector3 angles = transform.localEulerAngles;

        angles.z = orientation.y + 180;

        transform.localEulerAngles = angles;
    }

    private void SetVelocity()
    {
        speedVx = Mathf.Cos((transform.localEulerAngles.z + 90f) / 180f * Mathf.PI) * speed;
        speedVy = Mathf.Sin((transform.localEulerAngles.z + 90f) / 180f * Mathf.PI) * speed;
        speedVy += headWind;
    }

    private void IsBorderInner()
    {
        //if (transform.position.x < -border.x || transform.position.x > border.x || transform.position.y < -border.y)
        if(transform.position.y < -border.y)
            isDead = true;
    }

    private void GoNextScene()
    {
        SceneManager.LoadScene("KawazStageSelect");
    }

    public void DoBeginAnitmation1()
    {
        StartCoroutine(BeginAnimation1());
    }

    public void DoBeginAnimation2()
    {
        StartCoroutine(BeginAnimation2());
    }

    private IEnumerator BeginAnimation1()
    {
        actionEnabled = false;

        transform.DOMoveY(
            1.0f,
            startAnimationTime);

        yield return new WaitForSeconds(startAnimationTime);
        //actionEnabled = true;
    }

    private IEnumerator BeginAnimation2()
    {
        transform.DOMoveY(
            -3.0f,
            startAnimationTime / 2f);

        yield return new WaitForSeconds(startAnimationTime / 2f);

        actionEnabled = true;
        posY = -3.0f;
    }
}
