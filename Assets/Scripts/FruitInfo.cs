using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitInfo : MonoBehaviour
{
    [SerializeField, Tooltip("0 is smallest fruit")]
    private GameObject[] fruitPrefabs;

    private static FruitInfo instance;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
    }
    public static FruitInfo Instance()
    {
        return instance;
    }

    public GameObject GetFruitPrefabFromLevel(int fruitLevel)
    {
        GameObject newFruit = fruitPrefabs[fruitLevel];

        if (newFruit == null)
        {
            Debug.LogError("ERROR: a fruit corresponding to level '" + fruitLevel + "' does not exist");
            return null;
        }

        return newFruit;
    }
}
