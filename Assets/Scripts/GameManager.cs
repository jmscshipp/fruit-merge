using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField]
    private FruitPlacer fruitplacer;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndLevel()
    {
        // stop player interactivity here
        // and freeze all fruit where they are
        // add cool end game animation here? like fruit scattering everywhere
        fruitplacer.EndLevel();
        UIManager.Instance().OpenEndgamePopup();
    }

    public void ResetLevel()
    {
        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
        foreach (GameObject fruit in fruits)
            Destroy(fruit);

        // reset score

        fruitplacer.BeginLevel();
    }
}
