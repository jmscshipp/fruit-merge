using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionResolver : MonoBehaviour
{
    [SerializeField]
    private FruitPlacer fruitPlacer;

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

    // we already know these gameobjects are the same type of fruit
    public void ResolveCollision(GameObject obj1, GameObject obj2, Vector3 position, Fruit.Level fruitLevel)
    {
        // if one of the objs is null prolly cus another collision already happened involving one of em
        if (obj1 == null || obj2 == null)
            return;

        //if (fruitLevel == Fruit.Level.Watermelon)
            // I guess you win??
        
        obj1.GetComponent<Fruit>().Combine(position);
        obj2.GetComponent<Fruit>().Combine(position);

        fruitPlacer.PlaceFruit(++fruitLevel, position);
    }
}
