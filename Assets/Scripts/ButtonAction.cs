using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class ButtonAction : MonoBehaviour
{
    public KeyManager keyManager;
    public DiamondManager diamondManager;
    public UnityEngine.UI.Button monitorButton;
    public UnityEngine.UI.Button answerButton1;
    public UnityEngine.UI.Button answerButton2;
    public int initialDiamond = 50;
    public TextMeshProUGUI diamondText;

    void Update()
    {
        FinalDiamond();
    }

    public void OnClickWrong()
    {
        initialDiamond -= 10;
    }

    public void FinalDiamond()
    {
        diamondText.text = "+" + (initialDiamond * 10).ToString();
    }
    public void OnButtonClick()
    {
        keyManager.keyCount++;
        diamondManager.UpdateDiamondCount(initialDiamond);
    }

    public void DisableMonitor()
    {
        monitorButton.interactable = false;
    }

    public void DisableButton1()
    {
        answerButton1.interactable = false;
    }

    public void DisableButton2()
    {
        answerButton2.interactable = false;
    }
}