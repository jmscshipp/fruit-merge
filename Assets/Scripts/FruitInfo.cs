using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitInfo : MonoBehaviour
{
    public enum Level
    {
        None,
        Blueberry,
        Raspberry,
        Tangerine,
        Pomegranite,
        Orange,
        Apple,
        Peach,
        Honeydew,
        Watermelon
    }

    [SerializeField]
    private GameObject blueBerryPrefab;
    [SerializeField]
    private GameObject raspberryPrefab;
    [SerializeField]
    private GameObject tangerinePrefab;
    [SerializeField]
    private GameObject pomegranitePrefab;
    [SerializeField]
    private GameObject orangePrefab;
    [SerializeField]
    private GameObject applePrefab;
    [SerializeField]
    private GameObject peachPrefab;

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

    public GameObject GetFruitPrefabFromLevel(Level fruitLevel)
    {
        GameObject newFruit = null;

        switch (fruitLevel)
        {
            case Level.Blueberry:
                newFruit = blueBerryPrefab;
                break;
            case Level.Raspberry:
                newFruit = raspberryPrefab;
                break;
            case Level.Tangerine:
                newFruit = tangerinePrefab;
                break;
            case Level.Pomegranite:
                newFruit = pomegranitePrefab;
                break;
            case Level.Orange:
                newFruit = orangePrefab;
                break;
            case Level.Apple:
                newFruit = applePrefab;
                break;
            case Level.Peach:
                newFruit = peachPrefab;
                break;
        }

        return newFruit;
    }
}
