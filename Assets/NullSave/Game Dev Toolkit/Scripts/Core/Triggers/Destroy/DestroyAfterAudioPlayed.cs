using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("This component destroys the attached GameObject once the attacked AudioSource finishes playing. If the AudioSource is not already playing the component will wait for it to start and then complete.")]
    [RequireComponent(typeof(AudioSource))]
    public class DestroyAfterAudioPlayed : MonoBehaviour
    {

        #region Fields

        private AudioSource audioSource;
        private bool audioStarted;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!audioStarted) audioStarted = audioSource.isPlaying;

            if (audioStarted)
            {
                if (audioSource.time >= audioSource.clip.length || !audioSource.isPlaying)
                {
                    InterfaceManager.ObjectManagement.DestroyObject(gameObject);
                }
            }
        }

        #endregion

    }
}