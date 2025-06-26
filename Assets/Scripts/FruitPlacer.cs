using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private List<Vector3> fruitPlacements = new List<Vector3>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0f);
            GameObject newFruit = Instantiate(blueBerryPrefab, spawnPosition, Quaternion.identity);
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
        GameObject newFruit;
        switch (fruitLevel)
        {
            case Fruit.Level.Blueberry:
                newFruit = Instantiate(blueBerryPrefab, position, Quaternion.identity);
                break;
            case Fruit.Level.Raspberry:
                newFruit = Instantiate(raspberryPrefab, position, Quaternion.identity);
                break;
            case Fruit.Level.Tangerine:
                newFruit = Instantiate(tangerinePrefab, position, Quaternion.identity);
                break;
            case Fruit.Level.Pomegranite:
                newFruit = Instantiate(pomegranitePrefab, position, Quaternion.identity);
                break;
            case Fruit.Level.Orange:
                newFruit = Instantiate(orangePrefab, position, Quaternion.identity);
                break;
            case Fruit.Level.Apple:
                newFruit = Instantiate(applePrefab, position, Quaternion.identity);
                break;
            case Fruit.Level.Peach:
                newFruit = Instantiate(peachPrefab, position, Quaternion.identity);
                break;
        }

        yield return new WaitForEndOfFrame();
        fruitPlacements.Remove(position);
    }
}
