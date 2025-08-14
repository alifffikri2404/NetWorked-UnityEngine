using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NameInput : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public int maxCharacterLimit = 10;
    public TMP_Text errorMessage;

    void Start()
    {
        nameInputField.characterLimit = maxCharacterLimit;
        nameInputField.contentType = TMP_InputField.ContentType.Alphanumeric; // only alphanumeric characters
    }

    public void SaveNameInput()
    {
        string playerName = nameInputField.text.ToLower();
        PlayerPrefs.SetString("PlayerName", playerName);
    }

    public void ObjectiveScene()
    {
        string playerName = nameInputField.text.Trim();

        if (playerName == "")
        {
            errorMessage.text = "Input your name first!"; // Display an error message
        }
        else
        {
            SaveNameInput();
            SceneManager.LoadSceneAsync(1);
        }
    }

    public void ClearErrorText()
    {
        errorMessage.text = "";
    }
}
