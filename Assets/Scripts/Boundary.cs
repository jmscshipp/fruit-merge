using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Boundary : MonoBehaviour
{
    [SerializeField]
    private float overlapTimeMax = 8f; // time in seconds before level restart when fruit overlaps boundary
    private bool gameActive = true;
    private SpriteRenderer spriteRenderer;
    
    // for lerping in transparency of boundary sprite
    [SerializeField]
    private Color spriteColor; // for lerping purposes
    [SerializeField]
    private Color clearColor; // for lerping purposes
    private bool lerpingIn = false;
    private bool lerpingOut = false;
    private float lerpTimer = 0f;
    private bool visible = false;

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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.clear;
    }

    void Update()
    {
        if (!gameActive)
            return;

        // making sprite transparent when no fruits are overlapping
        //if (lerpingIn)
        //{
        //    lerpTimer += Time.deltaTime * 3f;
        //    spriteRenderer.color = Color.Lerp(Color.clear, clearColor, lerpTimer);
        //    if (lerpTimer >= 1f)
        //    {
        //        lerpingIn = false;
        //    }
        //}
        //else if (lerpingOut)
        //{
        //    lerpTimer += Time.deltaTime * 3f;
        //    spriteRenderer.color = Color.Lerp(spriteColor, clearColor, lerpTimer);
        //    if (lerpTimer >= 1f)
        //    {
        //        lerpingOut = false;
        //    }
        //}

        // increase time for any overlapping fruits
        foreach (CollisionEntry entry in collidingFruits)
            {
                entry.timeOverlapping += Time.deltaTime;
                if (entry.timeOverlapping > overlapTimeMax)
                {
                    GameManager.Instance().EndLevel();
                    break;
                }
            }
    }

    public void TrackFruit(Object fruit)
    {
        Debug.Log("tracking new fruit");
        collidingFruits.Add(new CollisionEntry(fruit.GetInstanceID(), 0f));
        if (!visible)
            LerpIn();
    }

    public void StopTrackingFruit(Object fruit)
    {

        bool succesfulRemove = collidingFruits.Remove(collidingFruits.FirstOrDefault(entry => entry.objectId == fruit.GetInstanceID()));
        if (succesfulRemove && collidingFruits.Count == 0 && visible)
        {
        Debug.Log("stopped tracking fruit");
            LerpOut();

        }
    }

    public void ClearAllTrackedFruits()
    {
        collidingFruits.Clear();
        if (visible)
            LerpOut();
    }

    // to prevent time from counting during pause menu
    public void Pause()
    {
        gameActive = false;
    }

    // to prevent time from counting during pause menu
    public void Resume()
    {
        gameActive = true;
    }

    private void LerpIn()
    {
        lerpingIn = true;
        lerpingOut = false;
        visible = true;
        lerpTimer = 0f;
    }

    private void LerpOut()
    {
        lerpingOut = true;
        lerpingIn = false;
        visible = false;
        lerpTimer = 0f;
    }

    // for use with out of bounds boundary
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fruit")
        {
            StopTrackingFruit(gameObject);
            //outline.StopblinkingRed();
            //StopBlinkingRed();
        }
    }

    // for use with out of bounds boundary
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fruit")
        {
            if (collision.gameObject.transform.position.y >= transform.position.y)
            {
                TrackFruit(gameObject);
                //outline.BlinkRed();
                //StartCoroutine(BlinkRed(0f));
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
