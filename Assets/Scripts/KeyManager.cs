using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyManager : MonoBehaviour
{
    public int keyCount;
    public int keyRemaining;
    public TextMeshProUGUI keyText;
    public TextMeshProUGUI remainKeyText;

    void Update()
    {
        keyText.text = keyCount.ToString();
        RemainingKey();
    }

    public void RemainingKey()
    {
        int initialKey = 10;
        keyRemaining = initialKey - keyCount;
        if (remainKeyText != null)
        {
            remainKeyText.text = keyRemaining.ToString() + " left";
        }
    }
}
