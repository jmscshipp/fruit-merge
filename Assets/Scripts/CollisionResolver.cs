using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionResolver : MonoBehaviour
{
    // keeping track of fruit placement requests to get rid of duplicates
    private List<Vector3> placementRequests = new List<Vector3>();

    private static CollisionResolver instance;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
    }

    public static CollisionResolver Instance()
    {
        return instance;
    }

    // called by fruit when they collide with another fruit of the same type
    public void ResolveCollision(GameObject obj1, GameObject obj2, Vector3 position, int fruitLevel)
    {
        // if one of the objs is null prolly cus another collision already happened involving one of em
        if (obj1 == null || obj2 == null)
            return;

        //if (fruitLevel == FruitInfo.Level.Watermelon)
        // I guess you win??
        fruitLevel++;
        obj1.GetComponent<Fruit>().Combine(position, fruitLevel);
        obj2.GetComponent<Fruit>().Combine(position, fruitLevel);

        PlaceFruit(fruitLevel, position);
    }

    // combining fruit into a new one
    public void PlaceFruit(int fruitLevel, Vector3 position)
    {
        // this is a duplicate request from the other side of a collision...
        if (placementRequests.Contains(position))
            return;

        placementRequests.Add(position);
        StartCoroutine(DelayedFruitPlace(fruitLevel, position));
    }

    private IEnumerator DelayedFruitPlace(int fruitLevel, Vector3 position)
    {
        yield return new WaitForSeconds(0.1f);

        GameObject newFruit = Instantiate(FruitInfo.Instance().GetFruitPrefabFromLevel(fruitLevel), position, Quaternion.identity);
        newFruit.GetComponent<Rigidbody2D>().simulated = true;

        int pointsFromFruit = FruitInfo.Instance().GetFruitPointValueFromLevel(fruitLevel);
        UIManager.Instance().CreateFruitPointText(pointsFromFruit, newFruit.transform.position);
        ScoreManager.Instance().AddToScore(pointsFromFruit);

        yield return new WaitForEndOfFrame();
        placementRequests.Remove(position);
    }
}
