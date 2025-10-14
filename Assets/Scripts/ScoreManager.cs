using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int currentLevelScore = 0;
    private int localHighScore = 0;
    private static ScoreManager instance;

    private bool newLocalHighScoreThisSession = false;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;

        localHighScore = PlayerPrefs.GetInt("LocalHighScore", 0);
    }
    public static ScoreManager Instance()
    {
        return instance;
    }

    private void Start()
    {
        ResetLevelScore();
    }

    public void AddToScore(int additionalScore)
    {
        currentLevelScore += additionalScore;
        UIManager.Instance().UpdateCurrentScoreText(currentLevelScore);

        if (currentLevelScore > localHighScore)
        {
            localHighScore = currentLevelScore;
            PlayerPrefs.SetInt("LocalHighScore", localHighScore);
            newLocalHighScoreThisSession = true;
        }
    }

    public void ResetLevelScore()
    {
        currentLevelScore = 0;
        localHighScore = PlayerPrefs.GetInt("LocalHighScore", 0);
        newLocalHighScoreThisSession = false;
        UIManager.Instance().UpdateCurrentScoreText(currentLevelScore);
        UIManager.Instance().UpdateHighScoreText(localHighScore);
    }

    public void ResetLocalHighScore()
    {
        localHighScore = 0;
        PlayerPrefs.SetInt("LocalHighScore", localHighScore);
        newLocalHighScoreThisSession = false;
        UIManager.Instance().UpdateHighScoreText(localHighScore);
    }

    public int GetCurrentLevelScore() => currentLevelScore;

    public int GetLocalHighScore() => localHighScore;
    public bool IsNewLocalHighScoreThisSession() => newLocalHighScoreThisSession;
}
