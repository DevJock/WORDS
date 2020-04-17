using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHelper : MonoBehaviour
{
    public bool buttonClicked = false;
    public string ButtonMessage { get; set; }
    string revertString;
    static string bMessage;
    static bool runHide = false;
    static bool runShow = false;
    static Button _button;


    public static void Clicked()
    {
        Button[] buttons = GameObject.FindObjectsOfType<Button>();
        foreach(Button button in buttons)
        {
            button.interactable = false;
        }
    }

    public static void ResetClick()
    {
        Button[] buttons = GameObject.FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    public static void Clicked2(Button button,string message = "Processing")
    {
        _button = button;
        bMessage = message;
        runHide = true;
    }

    public static void ResetClick2(Button button)
    {
        runShow = true;
    }


    private void Update()
    {
        if(runHide)
        {
            ButtonMessage = bMessage;
            _button.enabled = false;
            revertString = _button.GetComponentInChildren<Text>().text;
            _button.GetComponentInChildren<Text>().text = bMessage;
            runHide = false;
        }

        if(runShow)
        {
            _button.enabled = true; 
            _button.GetComponentInChildren<Text>().text = revertString;
            runShow = false;
        }
    }
}
