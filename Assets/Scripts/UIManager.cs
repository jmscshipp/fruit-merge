using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private EndgamePopupUI endgamePopup;
    [SerializeField]
    private GameObject fruitPointTextPrefab;
    [SerializeField]
    private TMP_Text currentScoreText;

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

    public void CreateFruitPointText(float pointValue, Vector3 fruitWorldPos)
    {
        Vector2 canvasPos = Camera.main.WorldToScreenPoint(fruitWorldPos);
        FruitPointTextUI fruitPointText= Instantiate(fruitPointTextPrefab, canvasPos, Quaternion.identity, transform).GetComponent<FruitPointTextUI>();
        fruitPointText.SetPointValue(pointValue);
        fruitPointText.Activate();
    }

    public void UpdateCurrentScoreText(int score)
    {
        currentScoreText.text = score.ToString();
    }
}
