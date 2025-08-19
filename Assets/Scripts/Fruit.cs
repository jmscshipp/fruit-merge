using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    // stuff for combining anim
    private bool lerping = false;
    private float lerpCounter = 0f;
    private Vector3 startPos;
    private Vector3 goalPos;
    private float startScale;
    private float goalScale;
    [SerializeField, Tooltip("Have to keep this the same as the actual sprite color!")]
    private Color color;
    private Color goalColor;

    // variable to prevent collision with boundary on the way into the pit
    private bool inGame = false;

    public int level;

    public int GetLevel() => level;

    private void Update()
    {
        if (lerping)
        {
            lerpCounter += Time.deltaTime * 10f;
            transform.position = Vector3.Lerp(startPos, goalPos, lerpCounter);
            transform.localScale = Vector3.Lerp(Vector3.one * startScale, Vector3.one * goalScale, lerpCounter);
            GetComponent<SpriteRenderer>().color = Color.Lerp(color, goalColor, lerpCounter);
            if (lerpCounter >= 1)
                Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fruit")
        {
            inGame = true;

            if (level == collision.gameObject.GetComponent<Fruit>().GetLevel())
                CollisionResolver.Instance().ResolveCollision(gameObject, collision.gameObject, collision.contacts[0].point, level);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boundary" && inGame)
        {
            // start countdown to restart level
            GameManager.Instance().EndLevel();
        }
    }

    public void PlayCombinationEffects(Vector3 goalPosition, int nextFruitLevel)
    {
        GetComponent<CircleCollider2D>().enabled = false;
        startScale = transform.localScale.x;
        startPos = transform.position;
        goalPos = goalPosition;
        goalScale = transform.localScale.x + 0.2f; // PLACEHOLDER.. should get scale of next fruit
        goalColor = FruitInfo.Instance().GetFruitPrefabFromLevel(nextFruitLevel).
            GetComponent<Fruit>().GetColor();
        lerping = true;
    }

    public Color GetColor() => color;
}
