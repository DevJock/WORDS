using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
   
    static bool draw = false;
    static bool hide = false;
    
    UIManager uiManager;
    private void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIManager>();
    }

    public static void ShowPanel()
    {
        draw = true;
    }


    public static void HidePanel()
    {
        hide = true;
    }

    private void Update()
    {
        if (draw)
        {
            drawPanel();
            draw = false;
        }

        if(hide)
        {
            undrawPanel();
            hide = false;
        }
    }


    private void drawPanel()
    {
        displayPanel();
    }

    private void displayPanel()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void undrawPanel()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void ACTION_EXITGAMEVIEW()
    {
        GameController.GAME.GameState = GameController.GAME.STATE.Exited;
        HidePanel();
        SceneManager.LoadScene(0);
        uiManager.SwitchPage(UIManager.Page.MainMenu);
    }

    public void ACTION_RESETGAME()
    {
        HidePanel();
        SceneManager.LoadScene(1);
    }
}
