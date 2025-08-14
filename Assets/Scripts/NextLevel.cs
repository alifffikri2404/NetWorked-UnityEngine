using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{

    public TimerManager timerManager;
    public DiamondManager diamondManager;

    public void LoadNextLevel(int levelIndex)
    {

        if (timerManager != null && diamondManager != null)
        {
            timerManager.RewardPlayer();
            SceneManager.LoadSceneAsync(levelIndex);
        }
        else
        {
            Debug.LogWarning("TimerManager or DiamondManager references not set!");
        }
    }
}
