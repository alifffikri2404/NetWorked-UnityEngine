using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public float initialTime = 420;
    public TextMeshProUGUI timerText;
    public DiamondManager diamondManager;
    public LostMenu lostMenu;
    public bool timerActive = true;
    public int reward;
    public float StoppedTime
    {
        get;
        private set;
    }
    public bool isStopped = false;

    void Start()
    {
        diamondManager = FindObjectOfType<DiamondManager>();
        if (diamondManager == null)
        {
            Debug.LogError("DiamondManager not found! Ensure it exists in the scene.");
        }
    }

    public void StopTimer()
    {
        if (!isStopped)
        {
            isStopped = true;
            StoppedTime = initialTime;
            timerActive = false;
        }
    }

    void Update()
    {
        if (timerActive && initialTime > 0)
        {
            initialTime -= Time.deltaTime;
            initialTime = Mathf.Max(initialTime, 0f);
        }

        if (initialTime <= 0)
        {
            RewardPlayer();
        }
        DisplayTime(initialTime);
    }

    void DisplayTime(float timeInSec)
    {
        int minutes = Mathf.FloorToInt(timeInSec / 60);
        int seconds = Mathf.FloorToInt(timeInSec % 60);

        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timeInSec <= 0)
        {
            timerText.text = "KO";
        }
        else
        {
            timerText.text = timeString;
        }
    }

    public void RewardPlayer()
    {
        int remainingTimeInSeconds = Mathf.FloorToInt(initialTime);
        if (remainingTimeInSeconds <= 0)
        {
            lostMenu.ShowPanel();
        }
        else
        {
            reward = remainingTimeInSeconds * 1;
            diamondManager.UpdateDiamondCount(reward);
        }
    }
}
