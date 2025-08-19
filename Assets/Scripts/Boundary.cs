using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class Boundary : MonoBehaviour
{
    [SerializeField]
    private float overlapTimeMax = 2f; // time in seconds before level restart when fruit overlaps boundary
    private bool gameActive = true; // marked false when game is over

    // for use keeping track of currently overlapping fruits
    class CollisionEntry
    {
        public int objectId;
        public float timeOverlapping;
        public CollisionEntry(int id, float time)
        {
            objectId = id;
            timeOverlapping = time;
        }
    }

    // collection of fruits currently overlapping boundary 
    private HashSet<CollisionEntry> collidingFruits = new HashSet<CollisionEntry>();

    void Update()
    {
        if (gameActive == false)
            return;

        // increase time for any overlapping fruits
        foreach (CollisionEntry entry in collidingFruits)
        {
            entry.timeOverlapping += Time.deltaTime;
            if (entry.timeOverlapping > overlapTimeMax)
                GameManager.Instance().EndLevel();
        }
    }

    public void TrackFruit(Object fruit)
    {
        collidingFruits.Add(new CollisionEntry(fruit.GetInstanceID(), 0f));
    }

    public void StopTrackingFruit(Object fruit)
    {
        collidingFruits.Remove(collidingFruits.FirstOrDefault(entry => entry.objectId == fruit.GetInstanceID()));
    }
}
