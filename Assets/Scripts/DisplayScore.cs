using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using System.IO;

public class DisplayScore : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText; 
    private Scoreboard scoreboard;

    void Start()
    {
        scoreboard = FindObjectOfType<Scoreboard>();

        // Obtain current or stored player name and diamond count
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");
        int diamondCount = PlayerPrefs.GetInt("DiamondCount", 0)*10;

        // Display player name and diamond count
        string pNameText = $"Congratulations {playerName}!\nYou just passed all challenges!\n";
        string pScoreText = $"{diamondCount}";

        string congratsMessage =
          $"You can view your previous score in the Scoreboard.\n" +
          $"You can play again if you want to beat\nyour previous score.";

        nameText.text = pNameText;
        scoreText.text = pScoreText;
        infoText.text = congratsMessage;
    }
}