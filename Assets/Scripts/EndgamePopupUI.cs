using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndgamePopupUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text currentScoreText;
    [SerializeField]
    TMP_Text scoreDescription;

    public void Open()
    {
        currentScoreText.text = ScoreManager.Instance().GetCurrentLevelScore().ToString();
        if (ScoreManager.Instance().IsNewLocalHighScoreThisSession())
            scoreDescription.text = "New High Score!";
        else
            scoreDescription.text = "Your score was";

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void RestartButton()
    {
        GameManager.Instance().ResetLevel();
        Close();
    }
}
