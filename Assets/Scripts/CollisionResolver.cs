using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionResolver : MonoBehaviour
{
    // keeping track of fruit placement requests to get rid of duplicates
    public HashSet<int> placementRequests = new HashSet<int>();

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

        // there's already a collision being resolved involving one of these fruits
        if (placementRequests.Contains(obj1.GetInstanceID()) || placementRequests.Contains(obj2.GetInstanceID()))
            return;

        //if (fruitLevel == FruitInfo.Level.Watermelon)
        // I guess you win??

        // disable collided fruits and play their effects
        fruitLevel++;
        obj1.GetComponent<Fruit>().PlayCombinationEffects(position, fruitLevel);
        obj2.GetComponent<Fruit>().PlayCombinationEffects(position, fruitLevel);

        // prevent further collisions with these fruits
        placementRequests.Add(obj1.GetInstanceID());
        placementRequests.Add(obj2.GetInstanceID());
        StartCoroutine(DelayedRemovePlacementRequest(obj1.GetInstanceID()));
        StartCoroutine(DelayedRemovePlacementRequest(obj2.GetInstanceID()));

        // instantiate and position new fruit as collision result
        GameObject newFruit = Instantiate(FruitInfo.Instance().GetFruitPrefabFromLevel(fruitLevel), position, Quaternion.identity);
        newFruit.GetComponent<Fruit>().PlayFruit(); // enable fruit physics and collision

        // scoring stuff
        int pointsFromFruit = FruitInfo.Instance().GetFruitPointValueFromLevel(fruitLevel);
        UIManager.Instance().CreateFruitPointText(pointsFromFruit, newFruit.transform.position);
        ScoreManager.Instance().AddToScore(pointsFromFruit);
    }

    // delayed removal of placement request hashset entry just to keep it from getting bloated
    private IEnumerator DelayedRemovePlacementRequest(int placementRequest)
    {
        yield return new WaitForSeconds(1f);
        placementRequests.Remove(placementRequest);
    }
}
