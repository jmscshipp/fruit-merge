using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text highscoreText;

    private void Start()
    {
        // open main menu on start
        Open();
    }

    public void Open()
    {
        gameObject.SetActive(true);

        // update high score text
        highscoreText.text = ScoreManager.Instance().GetLocalHighScore().ToString();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }   

    public void ContinueButton()
    {
        // resume level from save
    }

    public void PlayButton()
    {
        GameManager.Instance().ResetLevel();
        Close();
    }
}
