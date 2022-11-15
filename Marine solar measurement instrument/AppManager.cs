using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    [SerializeField] private TMP_Text stateText;

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Home))
            {
                //home button
                setStateText("Home");
            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                //back button
                setStateText("Esc");
                //Application.Quit();
            }
            else if (Input.GetKey(KeyCode.Menu))
            {
                //menu button
                setStateText("Menu");
            }
        }
    }

    void setStateText(string text)
    {
        if (stateText == null) return;
        stateText.text = text;
    }
}
