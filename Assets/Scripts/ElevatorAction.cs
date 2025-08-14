using UnityEngine;
using UnityEngine.UI;

public class ElevatorAction : MonoBehaviour
{
    public KeyManager keyManager;
    public GameObject rewardPanel;
    public GameObject insuffKeyPanel;
    public LevelReward levelReward;

    [Header("SFX")]
    [SerializeField] public AudioClip insuffSound;
    [SerializeField] public AudioClip rewardSound;

    public void OnElevatorButtonClick()
    {
        if (keyManager.keyCount == 10)
        {
            SoundManager.instance.PlaySound(rewardSound);
            rewardPanel.SetActive(true);
            if (levelReward != null)
            {
                levelReward.StopTimeButton();
            }
        }
        else
        {
            SoundManager.instance.PlaySound(insuffSound);
            insuffKeyPanel.SetActive(true);
        }
    }
}