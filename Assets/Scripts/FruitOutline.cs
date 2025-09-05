using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitOutline : MonoBehaviour
{
    private SpriteRenderer outline;

    // Start is called before the first frame update
    void Start()
    {
        outline = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Whitout()
    {
        outline.color = Color.white;
    }
}
