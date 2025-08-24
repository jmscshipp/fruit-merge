using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuPopupUI : MonoBehaviour
{
    // buttons that should only be active in regular gameplay
    [SerializeField]
    private GameObject resumeButton;
    [SerializeField]
    private GameObject restartButton;
    [SerializeField]
    private GameObject returnToMenuButton;
    [SerializeField]
    private GameObject exitFromGameButton;

    public void Open(bool inMainMenu = false)
    {
        gameObject.SetActive(true);
        if (inMainMenu)
        {
            resumeButton.SetActive(false);
            restartButton.SetActive(false);
            exitFromGameButton.SetActive(false);
            returnToMenuButton.SetActive(true);
        }
        else // in regular game play
        {
            resumeButton.SetActive(true);
            restartButton.SetActive(true);
            exitFromGameButton.SetActive(true);
            returnToMenuButton.SetActive(false);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void ResumeButton()
    {
        Close();
    }

    public void RestartButton()
    {
        GameManager.Instance().ResetLevel();
        Close();
    }

    public void ToggleAudiobutton()
    {
        // toggle audio
    }

    public void ToggleMusicButton()
    {
        // toggle music
    }

    public void ResetHighscoreButton()
    {
    
    }

    // for use exiting settings modal from the main menu
    public void ReturnToMenuButton()
    {
        Close();
    }

    // for use exiting to the main menu from normal gameplay
    public void ExitFromGameButton()
    {
        GameManager.Instance().EndLevel(true);
        Close();
        UIManager.Instance().GoToMainMenu();
    }
}
