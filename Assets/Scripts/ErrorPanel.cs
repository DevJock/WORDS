﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPanel : MonoBehaviour
{

    public static string Message { get; set; }
    static bool draw = false;
    public Button dismissButton;
    public Text messageText;


    public static void ShowError(string message, bool fire = true)
    {
        Message = message;
        draw = true;
    }

    private void Update()
    {
        if(draw)
        {
            drawPanel();
            draw = false;
        }
    }


    private void drawPanel()
    {
        messageText.text = Message;
        displayPanel();
    }

    private void displayPanel()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void hidePanel()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
