using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class ScoreSaver : MonoBehaviour
{
    public LeaderBoard leaderBoard = new LeaderBoard();
    public string teamName = "no name";
    // Start is called before the first frame update
    void Start()
    {
        /*
        for (int i = 0; i < 3; i++)
        {
            leaderBoard.scores.Add(new Score(i, 1.4f, "koitan"));
        }
        SaveScoreData(leaderBoard);
        */
        //SaveScoreData(leaderBoard);
        leaderBoard = LoadLeaderBoardData();

        //string jsonstr = JsonUtility.ToJson(leaderBoard);
        //Debug.Log(jsonstr);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveScoreData(LeaderBoard leaderBoard)
    {
        StreamWriter writer;

        string jsonstr = JsonUtility.ToJson(leaderBoard);

        writer = new StreamWriter(Application.dataPath + "/savedata.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    public LeaderBoard LoadLeaderBoardData()
    {
        string datastr = "";
        StreamReader reader;
        reader = new StreamReader(Application.dataPath + "/savedata.json");
        datastr = reader.ReadToEnd();
        reader.Close();

        return JsonUtility.FromJson<LeaderBoard>(datastr);
    }

    public void EntryScoreData(Score score)
    {
        leaderBoard.scores.Add(score);
        leaderBoard.scores.Sort((a, b) => (int)Mathf.Sign(a.time - b.time));
        if(leaderBoard.scores.Count>5)
        {
            leaderBoard.scores.RemoveAt(5);
        }
        for(int i=0;i<leaderBoard.scores.Count;i++)
        {
            leaderBoard.scores[i].rank = i + 1;
        }
        leaderBoard.UpdateDate = DateTime.Now.ToString();
        SaveScoreData(leaderBoard);
        Debug.Log("save score data");
    }

    public void EntryTeamName(string name)
    {
        teamName = name;
    }

    [ContextMenu("ClearLeaderBoard")]
    public void ClearLeaderBoardData()
    {
        SaveScoreData(new LeaderBoard());
    }
}

[System.Serializable]
public class Score
{
    public float time;
    public string name;
    public int rank;

    public Score(int _rank, float _time, string _name)
    {
        rank = _rank;
        time = _time;
        name = _name;
    }
}

[System.Serializable]
public class LeaderBoard
{
    public List<Score> scores = new List<Score>(5);
    public string UpdateDate;
}

