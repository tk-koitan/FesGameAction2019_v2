using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public float time = 0;
    private bool isRunning;
    public TextMeshProUGUI tmproText;
    public Animator canvasAnimator;
    static ScoreManager instance;
    public static ScoreManager Instatnce
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        canvasAnimator = GetComponent<Animator>();
        StartCoroutine(StageStart());
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning)
        {
            time += Time.deltaTime;
        }
        tmproText.text = time.ToString("0.00");
    }

    public void TimerStart()
    {
        isRunning = true;
    }

    public void TimerStop()
    {
        isRunning = false;
    }

    public void TimerReset()
    {
        time = 0;
    }

    private IEnumerator StageStart()
    {
        ActionInput.actionEnabled = false;
        yield return new WaitForSeconds(4);
        ActionInput.actionEnabled = true;
        TimerStart();
    }
}
