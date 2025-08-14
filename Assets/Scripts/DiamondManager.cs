using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DiamondManager : MonoBehaviour
{
    public TextMeshProUGUI diamondText;
    public int diamondCount;
    public int pointsPerDiamond = 10;
    public static DiamondManager instance;

    void Start()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 2)
        {
            ResetDiamondCount(); // Reset the diamond count for scene 2
        }
        else if (currentSceneIndex >= 3 && currentSceneIndex <= 5)
        {
            diamondCount = PlayerPrefs.GetInt("DiamondCount", 0); // Load diamond count for scenes 3, 4, and 5
        }
    }

    void Update()
    {
        diamondText.text = (diamondCount * pointsPerDiamond).ToString();
    }

    public void UpdateDiamondCount(int amount)
    {
        diamondCount += amount;
        PlayerPrefs.SetInt("DiamondCount", diamondCount);
    }

    public void ResetDiamondCount()
    {
        diamondCount = 0;
        PlayerPrefs.SetInt("DiamondCount", diamondCount);
    }
}