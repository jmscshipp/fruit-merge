using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FruitPlacer : MonoBehaviour
{
    [SerializeField, Tooltip("Bag of fruits for next fruit to be, more instances of one fruit will make it more likely to be chosen")]
    private List<FruitInfo.Level> randomFruitBag = new List<FruitInfo.Level>();

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

    private void Start()
    {
        heldFruit = CreateFruit(randomFruitBag[(int)Random.Range(0f, randomFruitBag.Count - 1)], crossHair.position);
        queuedFruit = CreateFruit(randomFruitBag[(int)Random.Range(0f, randomFruitBag.Count - 1)], transform.position);
        UpdateCrossHairBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        // set crosshair position
        float clampedX = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            leftXBoundary, rightXBoundary);
        crossHair.position = new Vector3(clampedX, crossHair.position.y, 0f);

        if (Input.GetKeyDown(KeyCode.Mouse0) && !lerping)
        { 
            ReleaseFruit();
        }

        if (lerping) // moving queued fruit to held fruit pos
        {
            lerpTimer += Time.deltaTime * 4f;
            heldFruit.transform.position = Vector2.Lerp(transform.position, crossHair.position, lerpTimer);
            queuedFruit.transform.position = Vector2.Lerp(transform.position + Vector3.up, transform.position, lerpTimer);
            if (lerpTimer >= 1f)
                lerping = false;
        }
        else // or having held fruit follow crosshair
        {
            if (heldFruit != null)
                heldFruit.transform.position = crossHair.position;
        }
    }

    // fruit currently held in crosshair goes down into the pit
    private void ReleaseFruit()
    {
        // releasing held fruit
        heldFruit.transform.position = crossHair.position; // teleport fruit to crosshair pos in case it was in the middle of lerping
        heldFruit.GetComponent<Rigidbody2D>().simulated = true;
        heldFruit = queuedFruit;
        UpdateCrossHairBoundaries();
        lerpTimer = 0f;
        lerping = true; // move held fruit from queued pos to held pos
        // load in a new fruit to be displayed at the top, ready to be loaded into crosshair
        queuedFruit = CreateFruit(randomFruitBag[(int)Random.Range(0f, randomFruitBag.Count - 1)], transform.position + Vector3.up);
    }

    // called by player when they place fruit
    public GameObject CreateFruit(FruitInfo.Level fruitLevel, Vector3 position)
    {
        return Instantiate(FruitInfo.Instance().GetFruitPrefabFromLevel(fruitLevel), position, Quaternion.identity);
    }

    private void UpdateCrossHairBoundaries()
    {
        leftXBoundary = -5.1f + heldFruit.transform.localScale.x / 2f;
        rightXBoundary = 5.1f - heldFruit.transform.localScale.x / 2f;
    }
}
