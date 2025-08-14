using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance
    {
        get;
        private set;
    }
    private AudioSource source;
    private bool isMuted = false;
    private float savedVolume = 0.435f;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ToggleSound()
    {
        if (isMuted)
        {
            source.volume = savedVolume;
            isMuted = false;
        }
        else
        {
            savedVolume = source.volume;
            source.volume = 0.0f;
            isMuted = true;
        }
    }

    public void PlaySound(AudioClip _sound)
    {
        if (!isMuted)
        {
            source.PlayOneShot(_sound);
        }
    }

    public void muteButton()
    {
        SoundManager.instance.ToggleSound();
    }
}
