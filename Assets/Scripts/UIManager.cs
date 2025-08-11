using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private EndgamePopupUI endgamePopup;

    private static UIManager instance;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
    }
    public static UIManager Instance()
    {
        return instance;
    }

    public void OpenEndgamePopup()
    {
        endgamePopup.Open();
    }

    public void CloseEndgamePopup()
    {
        endgamePopup.Close();
    }
}
