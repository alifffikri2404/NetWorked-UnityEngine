using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LostMenu : MonoBehaviour
{
    [SerializeField] GameObject lostMenu;
    [Header("SFX")]
    [SerializeField] public AudioClip lostSound;

    public void ShowPanel()
    {
        lostMenu.SetActive(true);
        Time.timeScale = 0;
        SoundManager.instance.PlaySound(lostSound);
    }

    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }
}
