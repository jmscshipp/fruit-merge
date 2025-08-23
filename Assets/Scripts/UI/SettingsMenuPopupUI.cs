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

    public void Open(bool inMainMenu = false)
    {
        gameObject.SetActive(true);
        if (inMainMenu)
        {
            resumeButton.SetActive(false);
            restartButton.SetActive(false);
        }
        else // in regular game play
        {
            resumeButton.SetActive(true);
            restartButton.SetActive(true);
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

    public void ExitToMenuButton()
    {
        GameManager.Instance().ResetLevel();
        Close();
    }
}
