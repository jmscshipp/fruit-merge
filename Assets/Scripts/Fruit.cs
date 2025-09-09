using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField, Tooltip("Have to keep this the same as the actual sprite color!")]
    private Color spriteColor;
    [SerializeField, Tooltip("The color being lerped to to blink when almost over border")]
    private Color dangerBlinkColor;
    private Boundary boundary; // assigned when collision occurs
    private SpriteRenderer spriteRenderer;
    private FruitOutline outline;
    private bool played = false; // true when fruit has been active in the pit
    public int level;
    [SerializeField]
    private AnimationCurve expandCurve; // animated scale of fruit when it gets played in the game
    private bool expandLerp = false;
    private float lerpTimer = 0f; // using for expanding AND blinking
    private bool blinkRed = false;
 
    public int GetLevel() => level;

    private void Awake()
    {
        boundary = GameObject.FindGameObjectWithTag("Boundary").GetComponent<Boundary>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        outline = GetComponentInChildren<FruitOutline>();
    }

    private void Update()
    {
        // making the fruit expand when placed at spawn time
        if (expandLerp)
        {
            lerpTimer += Time.deltaTime * 3f;
            transform.localScale = Vector3.one * expandCurve.Evaluate(lerpTimer);
            if (lerpTimer >= 1f)
            {
                GetComponent<Rigidbody2D>().simulated = true; // wait for fruit to be expanded to turn on physics
                expandLerp = false;
            }
        }
        else if (blinkRed)
        {
            lerpTimer += Time.deltaTime * 3f;
            spriteRenderer.color = Color.Lerp(spriteColor, dangerBlinkColor, Mathf.PingPong(lerpTimer, 1f));
        }
    }

    // called when fruit is entered into the game, either dropped from fruit placer or spawned from combination
    public void Play(bool createdFromCollision = false)
    {
        if (transform.position.y >= 3.69f) // if fruit is spawned in the pit, it won't be tracked
        {
            boundary.TrackFruit(gameObject);
            StartCoroutine(BlinkRed(0.5f));
        }
        played = true;
        if (createdFromCollision)
        {
            expandLerp = true;
            StartCoroutine(SuperImpose());
        }
        else
            GetComponent<Rigidbody2D>().simulated = true; // immediately use physics
    }

    // when fruit is first spawned in, move it to the front most sprite layer so it's in front
    private IEnumerator SuperImpose()
    {
        // set to superimposed sorting layer
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
            sprite.sortingLayerName = "SuperImposedFruit";
        yield return new WaitForSeconds(0.15f);
        // set back to default layer
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
            sprite.sortingLayerName = "Default";
    }

    // called when game is over or pauseed to prevent further action
    public void Freeze()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        foreach (CircleCollider2D collider in GetComponentsInChildren<CircleCollider2D>())
            collider.enabled = false;
    }

    // resume action after being frozen
    public void UnFreeze()
    {
        foreach (CircleCollider2D collider in GetComponentsInChildren<CircleCollider2D>())
            collider.enabled = true;
        if (played) // prevent fruit hanging in fruit placer from falling
            GetComponent<Rigidbody2D>().simulated = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fruit")
        {
            if (level == collision.gameObject.GetComponent<Fruit>().GetLevel())
                CollisionResolver.Instance().ResolveCollision(gameObject, collision.gameObject, collision.contacts[0].point, level);
        }
    }

    private IEnumerator BlinkRed(float delay)
    {
        yield return new WaitForSeconds(delay);
        lerpTimer = 0f;
        blinkRed = true;
    }

    private void StopBlinkingRed()
    {
        StopAllCoroutines();
        blinkRed = false;
        spriteRenderer.color = spriteColor;
    }

    // called when this fruit and another combine
    public void PlayCombinationEffects(Vector3 goalPosition, int nextFruitLevel)
    {
        Freeze();

        GetComponentInChildren<FruitOutline>().Whitout();
        spriteRenderer.color = Color.white;
        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(0.15f);
        boundary.StopTrackingFruit(gameObject); // remove from boundary collision list in case still overlapping
        Destroy(gameObject);
    }
}
