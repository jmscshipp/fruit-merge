using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
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

    // stuff for combining anim
    private bool lerping = false;
    private float lerpCounter = 0f;
    private Vector3 startPos;
    private Vector3 goalPos;
    private float startScale;
    private float goalScale;

    public Level level;

    public Level GetLevel() => level;

    private void Update()
    {
        if (lerping)
        {
            lerpCounter += Time.deltaTime * 10f;
            transform.position = Vector3.Lerp(startPos, goalPos, lerpCounter);
            transform.localScale = Vector3.Lerp(Vector3.one * startScale, Vector3.one * goalScale, lerpCounter);
            if (lerpCounter >= 1)
                Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fruit" &&
            level == collision.gameObject.GetComponent<Fruit>().GetLevel())
        {
            CollisionResolver.Instance().ResolveCollision(gameObject, collision.gameObject, collision.contacts[0].point, level);
        }
    }

    public void Combine(Vector3 goalPosition)
    {
        GetComponent<CircleCollider2D>().enabled = false;
        startScale = transform.localScale.x;
        startPos = transform.position;
        goalPos = goalPosition;
        goalScale = transform.localScale.x + 0.2f; // PLACEHOLDER.. should get scale of next fruit
        lerping = true;
    }
}
