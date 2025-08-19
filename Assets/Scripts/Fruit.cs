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

    private Boundary boundary; // assigned when collision occurs

    public int level;

    public int GetLevel() => level;

    private void Awake()
    {
        boundary = GameObject.FindGameObjectWithTag("Boundary").GetComponent<Boundary>();
    }

    private void Update()
    {
        if (lerping)
        {
            lerpCounter += Time.deltaTime * 10f;
            transform.position = Vector3.Lerp(startPos, goalPos, lerpCounter);
            transform.localScale = Vector3.Lerp(Vector3.one * startScale, Vector3.one * goalScale, lerpCounter);
            GetComponent<SpriteRenderer>().color = Color.Lerp(color, goalColor, lerpCounter);
            if (lerpCounter >= 1)
            {
                if (boundary != null)
                    boundary.StopTrackingFruit(gameObject); // remove from boundary collision list in case still overlapping
                Destroy(gameObject);
            }
        }
    }

    // called when fruit is entered into the game, either dropped from fruit placer or spawned from combination
    public void PlayFruit()
    {
        GetComponent<Rigidbody2D>().simulated = true;
        if (transform.position.y >= 4.8f) // if fruit is spawned in the pit, it won't be tracked
            boundary.TrackFruit(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fruit")
        {
            if (level == collision.gameObject.GetComponent<Fruit>().GetLevel())
                CollisionResolver.Instance().ResolveCollision(gameObject, collision.gameObject, collision.contacts[0].point, level);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boundary")
            boundary.StopTrackingFruit(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boundary")
        {
            if (transform.position.y >= 4.8f)
                boundary.TrackFruit(gameObject);
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
