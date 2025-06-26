using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FruitPlacer : MonoBehaviour
{
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

    [SerializeField, Tooltip("Bag of fruits for next fruit to be, more instances of one fruit will make it more likely to be chosen")]
    private List<Fruit.Level> randomFruitBag = new List<Fruit.Level>();

    // keeping track of fruit placement reqeusts to get rid of duplicates
    private List<Vector3> fruitPlacements = new List<Vector3>();

    [SerializeField]
    private Transform crossHair; // obj showing where next fruit will go

    // Update is called once per frame
    void Update()
    {
        crossHair.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, crossHair.position.y, 0f);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlaceFruit(randomFruitBag[(int)Random.Range(0f, randomFruitBag.Count - 1)], crossHair.position);
        }
    }

    // called by player when they place fruit and by collision resolver to combine fruits into a new one
    public void PlaceFruit(Fruit.Level fruitLevel, Vector3 position)
    {
        // this is a duplicate request from the other side of a collision...
        if (fruitPlacements.Contains(position))
            return;

        fruitPlacements.Add(position);
        StartCoroutine(DelayedFruitPlace(fruitLevel, position));
    }

    private IEnumerator DelayedFruitPlace(Fruit.Level fruitLevel, Vector3 position)
    {
        yield return new WaitForSeconds(0.1f);

        Instantiate(GetFruitPrefabFromLevel(fruitLevel), position, Quaternion.identity);

        yield return new WaitForEndOfFrame();
        fruitPlacements.Remove(position);
    }

    private GameObject GetFruitPrefabFromLevel(Fruit.Level fruitLevel)
    {
        GameObject newFruit = null;

        switch (fruitLevel)
        {
            case Fruit.Level.Blueberry:
                newFruit = blueBerryPrefab;
                break;
            case Fruit.Level.Raspberry:
                newFruit = raspberryPrefab;
                break;
            case Fruit.Level.Tangerine:
                newFruit = tangerinePrefab;
                break;
            case Fruit.Level.Pomegranite:
                newFruit = pomegranitePrefab;
                break;
            case Fruit.Level.Orange:
                newFruit = orangePrefab;
                break;
            case Fruit.Level.Apple:
                newFruit = applePrefab;
                break;
            case Fruit.Level.Peach:
                newFruit = peachPrefab;
                break;
        }

        return newFruit;
    }
}
