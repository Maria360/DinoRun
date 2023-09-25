using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;

public class ScoreController : MonoBehaviour
{
    DatabaseReference mDatabase;
    [SerializeField] private TMP_Text leaderboardTxt;
    string UserId;
    int score;

    void Awake()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
    }
    void Start()
    {
        //score = Manager.Instance.GetScore();
        GetUserScore();
    }

    public void WriteNewScore(int newScore)
    {
        mDatabase.Child("users").Child(UserId).Child("score").SetValueAsync(newScore);
    }
    public void GetUserScore()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("users/" + UserId + "/score")
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string _score = "" + snapshot.Value;
                score = int.Parse(_score);
                Debug.Log("Score: " + snapshot.Value);

                SetLabel();
            }
        });
    }
    public void SetLabel()
    {
        Debug.Log("Set Label");
        GameObject.Find("LabelScore").GetComponent<TMPro.TMP_Text>().text = "" + score;
    }
    public void UpdateScore(int value)
    {
        if (value > score)
        {
            score = value;
            WriteNewScore(score);
        }
        GetUsersHighestScores();
    }
    public void GetUsersHighestScores()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("users").OrderByChild("score").LimitToLast(5)
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var userDoc in (Dictionary<string, object>)snapshot.Value)
                {
                    var userObject = (Dictionary<string, object>)userDoc.Value;

                    //Debug.Log(userObject["username"] + " : " + userObject["score"]);
                    Debug.Log("LEADERBOARD: " + userObject["username"] + " : " + userObject["score"]);
                }
            }
        });
    }

    /*public void IncrementScore()
    {
        score += 100;
        SetLabel();
        WriteNewScore(score);
    }*/
    public class UserData
    {
        public int score;
        public string username;
    }
}
