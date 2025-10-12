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

    private void OnTriggerStay2D(Collider2D collision)
    {
        Transform fruit = collision.transform.parent; // the collider is a child of the fruit object
        if (fruit.tag == "Fruit")
        {
            Debug.Log("fruit hit boundary");

            fruit.GetComponent<Fruit>().UpdateFruitOutOfBounds(false);
            //StopTrackingFruit(gameObject);
            //outline.StopblinkingRed();
            //StopBlinkingRed();
        }
    }

    // for use with out of bounds boundary
    private void OnTriggerExit2D(Collider2D collision)
    {
        Transform fruit = collision.transform.parent; // the collider is a child of the fruit object
        if (fruit.tag == "Fruit")
        {
            if (fruit.position.y >= transform.position.y)
            {
                Debug.Log("fruit left boundary");

                fruit.GetComponent<Fruit>().UpdateFruitOutOfBounds(true);

                //TrackFruit(gameObject);
                //outline.BlinkRed();
                //StartCoroutine(BlinkRed(0f));
            }
        }
    }
}
