using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayName : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;

    void Start()
    {
        string playerName = PlayerPrefs.GetString("PlayerName");
        if (playerNameText != null)
        {
            playerNameText.text = "Welcome, " + playerName + "!";
        }
    }
}