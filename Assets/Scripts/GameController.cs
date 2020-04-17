using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public class GameData
    {
        public static string gameVersion = "1.2";
        public static string gameBuildType = "alpha";
        public static string gamePlatform = "Words_"+Application.platform.ToString();
        public static string enabled = "false";
    }

    public class MyScore
    {
        public static int HighScore { get; set; }
        public static int CurrentScore { get; set; }
    }

    public class GAME
    {
        public enum STATE
        {
            GameOver,
            Playing,
            Paused,
            Exited
        }
        public static STATE GameState { get; set; }
    }


    public static bool disableOnline;
    public GameObject player;
    UIManager uIManager;
    OnlineManager onlineManager;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        uIManager = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIManager>();
        onlineManager = GameObject.FindGameObjectWithTag("OnlineController").GetComponent<OnlineManager>();
        DontDestroyOnLoad(this);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        uIManager.SwitchPage(UIManager.Page.SignIn);
    }

    private void MainMenu()
    {
        uIManager.SwitchPage(UIManager.Page.MainMenu);
    }

    public void StartGame()
    {
        uIManager.SwitchPage(UIManager.Page.Game);
        SceneManager.LoadScene("gameScene");
    }

    public static void TOGGLE_PAUSE()
    {
        if(GAME.GameState == GAME.STATE.Playing)
        {
            GAME.GameState = GAME.STATE.Paused;
        }
        else if(GAME.GameState == GAME.STATE.Paused)
        {
            GAME.GameState = GAME.STATE.Playing;
        }
    }



    public static void GAMEOVER()
    {
        if (GAME.GameState == GAME.STATE.Playing)
        {
            GAME.GameState = GAME.STATE.GameOver;
        }
    }

    public void GAMESTART()
    {
        if(GAME.GameState == GAME.STATE.GameOver)
        {
            GAME.GameState = GAME.STATE.Playing;
        }
    }


    public void UploadScoreData()
    {
        StartCoroutine(onlineManager.uploadScoreData());
    }


    public void Exit()
    {
        ButtonHelper.Clicked();
        Application.Quit();
    }


    
}
