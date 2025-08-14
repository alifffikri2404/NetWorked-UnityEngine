using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class LevelReward : MonoBehaviour
{
    public TextMeshProUGUI timerRemain;
    public TextMeshProUGUI diamondReward;
    public TextMeshProUGUI diamondBonus;
    public DiamondManager diamondManager;
    public TimerManager timerManager;
    public float stoppedTime;

    public void Update()
    {
        DiamondReward();
        BonusDiamond();
    }

    public void DiamondReward()
    {
        if (diamondManager != null)
        {
            int totalDiamonds = diamondManager.diamondCount;
            diamondReward.text = "+" + (totalDiamonds * diamondManager.pointsPerDiamond).ToString();
        }
    }

    public void StopTimeButton()
    {
        if (timerManager != null)
        {
            timerManager.StopTimer();
            TimerBonus();
        }
    }

    public void TimerBonus()
    {
        if (timerManager != null)
        {
            float stoppedTime = timerManager.StoppedTime;
            DisplayTime(stoppedTime);
            BonusDiamond();
        }
    }

    public void BonusDiamond()
    {
        if (timerManager != null && diamondManager != null)
        {
            stoppedTime = timerManager.StoppedTime;
            int remainingTimeInSeconds = Mathf.FloorToInt(stoppedTime);
            diamondBonus.text = "+" + (remainingTimeInSeconds * diamondManager.pointsPerDiamond).ToString();
        }
    }

    public void DisplayTime(float stoppedTime)
    {
        int minutes = Mathf.FloorToInt(stoppedTime / 60);
        int seconds = Mathf.FloorToInt(stoppedTime % 60);

        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerRemain.text = timeString;
    }
}
