using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
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
