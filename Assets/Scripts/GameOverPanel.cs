using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{

    static bool draw = false;
    static bool hide = false;
    GameController gameController;
    UIManager uiManager;
    static string _score;
    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        uiManager = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIManager>();
    }

    public static void ShowPanel(string score)
    {
        draw = true;
        _score = score;
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

        if (hide)
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
        gameController.UploadScoreData();
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(1).Find("myscore").GetComponent<Text>().text = _score;
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
