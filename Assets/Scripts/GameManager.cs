using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Static instance for easy access
    public static GameManager instance;

    public int score;
    public float timer = 300f;

    // private void
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

/*    void Start()
    {
        StartNewGame();
    }

    void StartNewGame()
    {
        PlayerPrefs.SetString("PlayerName", "");
        PlayerPrefs.SetInt("DiamondCount", 0);
    }*/
}
