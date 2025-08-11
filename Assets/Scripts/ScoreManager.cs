using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int currentLevelScore = 0;

    private static ScoreManager instance;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
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
    }

    public void ResetLevelScore()
    {
        currentLevelScore = 0;
        UIManager.Instance().UpdateCurrentScoreText(currentLevelScore);
    }
}
