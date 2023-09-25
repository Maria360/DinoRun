using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Xml;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] private TMP_Text scoreTxt;
    [SerializeField] private float initialScrollSpeed;
    [SerializeField] private TextMeshProUGUI[] leaderBoardTxt;
    //[SerializeField] private List<TextMeshProUGUI> testTexts;


    private int score;
    private int bestScore;
    private float timer;
    private float scrollSpeed;

    public static Manager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        GetUserScore();
    }
    void Update()
    {
        SetScore();
        UpdateSpeed();
    }
    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);

        if (bestScore > score)
        {
            score = bestScore;
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).
            Child("score").SetValueAsync(score);
        }
        else if (bestScore < score)
        {
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).
            Child("score").SetValueAsync(score);
        }
        GetUsersHighestScores();


    }
    public void RestartScene()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;
    }
    public void SetScore()
    {
        int scorePerSec = 10;
        timer += Time.deltaTime;

        score = (int)(timer * scorePerSec);
        scoreTxt.text = string.Format("{0:00000}", score);
    }
    public void GetUserScore()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + "/score")
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
                bestScore = int.Parse(_score);

                //Debug.Log("Score: " + snapshot.Value);
            }
        });
    }
    public void GetUsersHighestScores()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("users").OrderByChild("score").LimitToLast(3)
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
                    var userObject = ((Dictionary<string, object>)userDoc.Value);

                    //testTexts.Add = "" + userObject["username"] + ":" + userObject["score"];
                    //testText.text = "Hola";

                    Debug.Log("LEADERBOARD: " + userObject["username"] + " : " + userObject["score"]);

                }

            }
        });
    }
    public float GetScrollSpeed()
    {
        return scrollSpeed;
    }

    private void UpdateSpeed()
    {
        float speedDivider = 10f;
        scrollSpeed = initialScrollSpeed + timer / speedDivider;
    }
}