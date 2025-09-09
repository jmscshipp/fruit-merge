using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitOutline : MonoBehaviour
{
    [SerializeField, Tooltip("Have to keep this the same as the actual sprite color!")]
    private Color spriteColor;
    [SerializeField, Tooltip("The color being lerped to to blink when almost over border")]
    private Color dangerBlinkColor;
    private SpriteRenderer outline;
    private bool blinking = false;
    private float lerpTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        outline = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //if (blinking)
        //{
        //    lerpTimer += Time.deltaTime * 3f;
        //    outline.color = Color.Lerp(Color.black, Color.red, Mathf.PingPong(lerpTimer, 1f));
        //}
    }

    public void BlinkRed()
    {
        lerpTimer = 0f;
        blinking = true;
    }
    public void StopblinkingRed()
    {
        blinking = false;
        outline.color = Color.black;
    }

    public void Whitout()
    {
        outline.color = Color.white;
    }
}
