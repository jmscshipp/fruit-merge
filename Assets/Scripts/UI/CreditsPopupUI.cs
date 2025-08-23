using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPopupUI : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    // add buttons on our names for links!
}
