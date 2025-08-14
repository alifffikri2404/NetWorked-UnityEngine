using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject PanelQuestion;

    public void OpenPanel()
    {
        if(PanelQuestion != null)
        {
            PanelQuestion.SetActive(true);
        }
    }
}
