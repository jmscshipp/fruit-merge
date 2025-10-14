using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FruitPlacer : MonoBehaviour
{
    [SerializeField, Tooltip("Bag of fruits for next fruit to be, more instances of one fruit will make it more likely to be chosen")]
    private List<int> randomFruitBag = new List<int>();

    [SerializeField]
    private Transform crossHair; // obj showing where next fruit will go
    private GameObject heldFruit; // fruit in crosshair waiting to be dropped in
    private GameObject queuedFruit; // fruit in waiting area ready to be held

    // limits of where the crosshair can go
    private float leftXBoundary = 0f;
    private float rightXBoundary = 0f;

    // lerping queued fruit into held fruit pos
    private bool lerping = false;
    private float lerpTimer = 0f;
    [SerializeField]
    private AnimationCurve lerpCurve;

    private bool canInteract = false;

    // Update is called once per frame
    void Update()
    {
        if (!canInteract)
            return;

        // set crosshair position
        float clampedX = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            leftXBoundary, rightXBoundary);
        crossHair.position = new Vector3(clampedX, crossHair.position.y, 0f);

        if (Input.GetKeyUp(KeyCode.Mouse0) && !lerping 
            && Camera.main.ScreenToWorldPoint(Input.mousePosition).y < 7.45f) // hmm this may need to be optimized later
        {
            ReleaseFruit();
        }

        if (lerping) // moving queued fruit to held fruit pos
        {
            lerpTimer += Time.deltaTime * 3f;
            heldFruit.transform.position = Vector2.Lerp(transform.position, crossHair.position, lerpCurve.Evaluate(lerpTimer));
            queuedFruit.transform.position = Vector2.Lerp(transform.position + Vector3.up, transform.position, lerpCurve.Evaluate(lerpTimer));
            if (lerpTimer >= 1f)
                lerping = false;
        }
        else // or having held fruit follow crosshair
        {
            if (heldFruit != null)
                heldFruit.transform.position = crossHair.position;
        }
    }

    // called by game manager when gameplay starts
    public void BeginLevel()
    {
        // reset old fruits if there are any
        if (heldFruit != null)
            Destroy(heldFruit);
        if (queuedFruit != null)
            Destroy(queuedFruit);

        heldFruit = CreateFruit(randomFruitBag[(int)Random.Range(0f, randomFruitBag.Count - 1)], crossHair.position);
        queuedFruit = CreateFruit(randomFruitBag[(int)Random.Range(0f, randomFruitBag.Count - 1)], transform.position);
        UpdateCrossHairBoundaries();
        lerpTimer = 0f;
        StartCoroutine(MakeInteractable());
    }

    // small delay so a fruit isn't placed when play button clicked
    private IEnumerator MakeInteractable()
    {
        yield return new WaitForSeconds(0.1f);
        canInteract = true;
    }

    // called by game  manager when player loses
    public void EndLevel()
    {
        canInteract = false;
        lerping = false;
    }

    // fruit currently held in crosshair goes down into the pit
    private void ReleaseFruit()
    {
        // releasing held fruit
        heldFruit.transform.position = crossHair.position; // teleport fruit to crosshair pos in case it was in the middle of lerping
        heldFruit.GetComponent<Fruit>().Play(); // enable fruit physics and collision
        heldFruit.GetComponent<Rigidbody2D>().velocity = Vector3.down * 8f;
        heldFruit = queuedFruit;
        UpdateCrossHairBoundaries();
        lerpTimer = 0f;
        lerping = true; // move held fruit from queued pos to held pos
        // load in a new fruit to be displayed at the top, ready to be loaded into crosshair
        queuedFruit = CreateFruit(randomFruitBag[(int)Random.Range(0f, randomFruitBag.Count - 1)], transform.position + Vector3.up);
    }

    // called by player when they place fruit
    public GameObject CreateFruit(int fruitLevel, Vector3 position)
    {
        GameObject newFruit = Instantiate(FruitInfo.Instance().GetFruitPrefabFromLevel(fruitLevel), position, Quaternion.identity);
        newFruit.name = "" + (int)Random.Range(0f, 20f);
        return newFruit;
    }

    private void UpdateCrossHairBoundaries() // need to come back here and fix boundaries based on fruit sizes
    {
        // might need to revisit this if using multiple types of coliders on the same fruit for more complex shapes
        float colliderRadius = heldFruit.GetComponent<CircleCollider2D>().radius;
        leftXBoundary = -5.15f + colliderRadius;
        rightXBoundary = 5.15f - colliderRadius;
    }

    // to limit interaction so when mouse is on top of screen, player can click UI without placing fruit
    public void SetInteractability(bool interactable)
    {
        canInteract = interactable;
    }

    // freeze game during pause menu
    public void Pause()
    {
        canInteract = false;
        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
        foreach (GameObject fruit in fruits)
            fruit.GetComponent<Fruit>().Freeze();
    }

    // unfreeze game after pause menu
    public void Resume()
    {
        Debug.Log("resuming fruit placer");
        canInteract = true;
        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
        foreach (GameObject fruit in fruits)
            fruit.GetComponent<Fruit>().UnFreeze();
    }
}
