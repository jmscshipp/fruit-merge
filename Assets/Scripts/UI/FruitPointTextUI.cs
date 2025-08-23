using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FruitPointTextUI : MonoBehaviour
{
    private bool activated = false;
    private float activationTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 20f);
            activationTime += Time.deltaTime;
            if (activationTime >= 1f)
                Destroy(gameObject);
        }
    }

    public void SetPointValue(float pointValue)
    {
        GetComponent<TMP_Text>().text = "+" + pointValue.ToString();
    }

    public void Activate()
    {
        activated = true;
    }
}
