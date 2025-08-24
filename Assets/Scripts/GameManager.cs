using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField]
    private FruitPlacer fruitplacer;
    [SerializeField]
    private Boundary boundary;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
    }
    public static GameManager Instance()
    {
        return instance;
    }

    // called when game is lost
    public void EndLevel(bool skipToMainMenu = false)
    {
        // add cool end game animation here? like fruit scattering everywhere

        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
        foreach (GameObject fruit in fruits)
            fruit.GetComponent<Fruit>().Freeze();

        fruitplacer.EndLevel();
        boundary.ClearAllTrackedFruits();

        if (skipToMainMenu)
            return;

        UIManager.Instance().OpenEndgamePopup();
    }

    // called when reset button is pressed
    public void ResetLevel()
    {
        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
        foreach (GameObject fruit in fruits)
            Destroy(fruit);

        // clearing again because when fruits are 
        // deleted, they get tracked by boundary from leavin the trigger
        boundary.ClearAllTrackedFruits(); 

        ScoreManager.Instance().ResetLevelScore();
        fruitplacer.BeginLevel();
    }
}
