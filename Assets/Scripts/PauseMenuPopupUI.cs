using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuPopupUI : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
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

    public void ExitToMenuButton()
    {
        GameManager.Instance().ResetLevel();
        Close();
    }
}
