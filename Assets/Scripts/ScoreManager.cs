using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public float time = 0;
    private bool isRunning;
    public TextMeshProUGUI tmproText;
    public TextMeshProUGUI tmproRanking;
    public TMP_InputField tmproInput;
    public Animator canvasAnimator;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip startSE;
    [SerializeField]
    private AudioClip countdownSE;
    private ScoreSaver saver;
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
        audioSource = GetComponent<AudioSource>();
        saver = GetComponent<ScoreSaver>();
        StartCoroutine(StageStart());
        //MusicManager.Play(MusicManager.Instance.bgm1);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            time += Time.deltaTime;
        }
        tmproText.text = time.ToString("0.00");
        var scores = saver.leaderBoard.scores;
        string rankingstr = "- Ranking -\n";
        for (int i = 0; i < scores.Count; i++)
        {
            rankingstr += scores[i].rank + ":  " + scores[i].time.ToString("0.00") + "  " + scores[i].name + "\n";
        }
        rankingstr += saver.leaderBoard.UpdateDate;
        tmproRanking.text = rankingstr;
    }

    public void TimerStart()
    {
        isRunning = true;
    }

    public void TimerStop(bool isSave = false)
    {
        isRunning = false;
        if (isSave)
        {
            //saver.EntryScoreData(new Score(1, time, saver.teamName));
        }
    }

    public void TimerReset()
    {
        time = 0;
    }

    private IEnumerator StageStart()
    {
        MusicManager.Stop();
        ActionInput.actionEnabled = false;
        yield return new WaitForSeconds(0.3f);
        audioSource.PlayOneShot(countdownSE);
        yield return new WaitForSeconds(1.0f);
        audioSource.PlayOneShot(countdownSE);
        yield return new WaitForSeconds(1.0f);
        audioSource.PlayOneShot(countdownSE);
        yield return new WaitForSeconds(1.0f);
        audioSource.PlayOneShot(startSE);
        yield return new WaitForSeconds(0.7f);
        ActionInput.actionEnabled = true;
        TimerStart();
        MusicManager.Play(MusicManager.Instance.bgm1);
    }

    public void EntryTeamName()
    {
        saver.teamName = tmproInput.text;
    }
}
