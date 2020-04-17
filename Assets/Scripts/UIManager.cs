using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum Page
    {
        Register,
        SignIn,
        HighScore,
        MainMenu,
        About,
        Options,
        Game
    };
    public Page currentPage;
    public GameObject text;
    public GameObject signIn;
    public GameObject register;
    public GameObject mainMenu;
    public GameObject HighScores;
    public GameObject Game;
    public float wordShowRate;
    OnlineManager onlineManager;
    GameController gameController;
    EventSystem system;
    float t;

    void Start()
    {
        system = EventSystem.current;
        t = wordShowRate;
    }

    private void Awake()
    {
        onlineManager = GameObject.FindGameObjectWithTag("OnlineController").GetComponent<OnlineManager>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        GameObject.Find("version").GetComponent<Text>().text = GameController.GameData.gamePlatform + "_" + GameController.GameData.gameVersion + "_" + GameController.GameData.gameBuildType;
        DontDestroyOnLoad(this);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }


    public void ACTION_SHOW_LOGIN()
    {
        ButtonHelper.Clicked();
        SwitchPage(Page.SignIn);
    }

    public void ACTION_SHOW_REGISTER()
    {
        ButtonHelper.Clicked();
        SwitchPage(Page.Register);
    }

    public void SHOW_SKIP_ACCOUNT()
    {
        ButtonHelper.Clicked();
        AlertPanel.ShowError("Skipping Account Sign In/ Sign Up Will disable High Score Tracking.\nAre You Sure you Want to Proceed ?");
        ButtonHelper.ResetClick();
    }

    public void ACTION_SKIP_ACCOUNT()
    {
        ButtonHelper.Clicked();
        GameController.disableOnline = true;
        OnlineManager.UserData.EmailID = "anonymous@anonymous.com";
        OnlineManager.UserData.Password = "anonymous";
        StartCoroutine(onlineManager.Login());
        AlertPanel.DismissPanel();
        ButtonHelper.ResetClick();
    }


    public void ACTION_SHOW_MAINMENU()
    {
        ButtonHelper.Clicked();
        SwitchPage(Page.MainMenu);
    }

    public void ACTION_SHOW_HIGHSCORES()
    {
        ButtonHelper.Clicked();
        SwitchPage(Page.HighScore);
    }


    public void SHOW_LEADERBOARD()
    {
        ButtonHelper.Clicked();
        StartCoroutine(onlineManager.downloadScoreData());
        ButtonHelper.ResetClick();
    }


    public void showLeaderboardData(List<string> gamerIDs, List<string> highScores)
    {
        if(GameController.disableOnline)
        {
            int index = gamerIDs.IndexOf("Player");
            gamerIDs.RemoveAt(index);
            highScores.RemoveAt(index);
        }
        for (int i = 0; i < highScores.Count - 1; i++)
        {
            for (int j = i + 1; j < highScores.Count; j++)
            {
                if (Convert.ToInt32(highScores[j]) > Convert.ToInt32(highScores[i]))
                {
                    string gt = gamerIDs[i];
                    gamerIDs[i] = gamerIDs[j];
                    gamerIDs[j] = gt;

                    string st = highScores[i];
                    highScores[i] = highScores[j];
                    highScores[j] = st;
                }
            }
        }
        HighScores.transform.Find("sView").GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "";
        for (int i=0;i<gamerIDs.Count;i++)
        {
            HighScores.transform.Find("sView").GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text += gamerIDs[i] + "\t\t\t" + highScores[i]+"\n";
        }
        
    }


    public void ACTION_LOGIN()
    {
        ButtonHelper.Clicked();
        string email = signIn.transform.Find("email").GetComponent<InputField>().text;
        string password = signIn.transform.Find("password").GetComponent<InputField>().text;
        OnlineManager.UserData.EmailID = email;
        OnlineManager.UserData.Password = password;
        StartCoroutine(onlineManager.Login());
    }

    public void ACTION_REGISTER()
    {
        ButtonHelper.Clicked();
        string name = register.transform.Find("name").GetComponent<InputField>().text;
        string email = register.transform.Find("email").GetComponent<InputField>().text;
        string password = register.transform.Find("password").GetComponent<InputField>().text;
        string gamerID = register.transform.Find("gamerID").GetComponent<InputField>().text;
        string validEmail = FormValidation.ValidEmail(email);
        string validName = FormValidation.ValidName(name);
        string validPassword = FormValidation.ValidPassword(password);
        string validGamerID = FormValidation.ValidGamerID(gamerID);


        if (validName != null)
        {
            ErrorPanel.ShowError(validName);
            ButtonHelper.ResetClick();
        }

        else if (validGamerID != null)
        {
            ErrorPanel.ShowError(validGamerID);
            ButtonHelper.ResetClick();
        }

        else if (validEmail != null)
        {
            ErrorPanel.ShowError(validEmail);
            ButtonHelper.ResetClick();
        }

        else if (validPassword != null)
        {
            ErrorPanel.ShowError(validPassword);
            ButtonHelper.ResetClick();
        }

        else
        {

            OnlineManager.UserData.Name = name;
            OnlineManager.UserData.EmailID = email;
            OnlineManager.UserData.Password = password;
            OnlineManager.UserData.GamerID = gamerID;
            StartCoroutine(onlineManager.Register());
        }
    }


    private void Update()
    {
        if(Game.activeSelf)
        {
            Game.transform.Find("Score").GetComponent<Text>().text = "Score: " + GameController.MyScore.CurrentScore ?? "0";
            Game.transform.Find("Timer").GetComponent<Text>().text = "Time Left: " + WordGame.Timer ?? "0.0";
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next  = null;
            if (system.currentSelectedGameObject)
            {
                next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            }
            if (next != null)
            {

                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                    inputfield.OnPointerClick(new PointerEventData(system));

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }

        }


        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if(signIn.activeSelf)
            {
                ACTION_LOGIN();
            }
            else if(register.activeSelf)
            {
                ACTION_REGISTER();
            }
        }


        if (mainMenu.activeSelf  || HighScores.activeSelf)
        {
            if (t <= 0.0f)
            {
                NextWord();
                t = wordShowRate;
            }
            t -= Time.deltaTime;
        }
        
    }

    private void NextWord()
    {
        GameObject word = WordManager.NewWord;
        float randX = UnityEngine.Random.Range(-0.3f, 0.3f);
        word.transform.position = new Vector3(randX, 1.3f, -9.3f);
        word.transform.rotation = new Quaternion(0, 0, 0, 0);
        word.transform.localScale = new Vector3(2, 2, 2);
        word.AddComponent<LightShow>().colorRate = UnityEngine.Random.Range(2, 5);
        Rigidbody rigid = word.AddComponent<Rigidbody>();
        Move move = word.AddComponent<Move>();
        move.speed = 0.0015f;
        Destroy(word, 10);
    }

    public void SwitchPage(Page page)
    {
        switch (page)
        {
            case Page.Register:
                {
                    if(!text.activeSelf)
                    {
                        text.SetActive(true);
                    }
                    register.SetActive(true);
                    signIn.SetActive(false);
                    mainMenu.SetActive(false);
                    Game.SetActive(false);
                    ButtonHelper.ResetClick();
                }
                break;
            case Page.SignIn:
                {
                    if (!text.activeSelf)
                    {
                        text.SetActive(true);
                    }
                    register.SetActive(false);
                    signIn.SetActive(true);
                    HighScores.SetActive(false);
                    mainMenu.SetActive(false);
                    Game.SetActive(false);
                    ButtonHelper.ResetClick();
                }
                break;
            case Page.MainMenu:
                {
                    if (!text.activeSelf)
                    {
                        text.SetActive(true);
                    }
                    register.SetActive(false);
                    signIn.SetActive(false);
                    mainMenu.SetActive(true);
                    HighScores.SetActive(false);
                    mainMenu.transform.Find("gamerID").GetComponent<Text>().text =OnlineManager.UserData.GamerID;
                    Game.SetActive(false);
                    ButtonHelper.ResetClick();
                }
                break;
            case Page.HighScore:
                {
                    if (!text.activeSelf)
                    {
                        text.SetActive(true);
                    }
                    register.SetActive(false);
                    signIn.SetActive(false);
                    mainMenu.SetActive(false);
                    HighScores.SetActive(true);
                    Game.SetActive(false);
                    ButtonHelper.ResetClick();
                }
                break;
            case Page.Game:
                {
                    text.SetActive(false);
                    register.SetActive(false);
                    signIn.SetActive(false);
                    mainMenu.SetActive(false);
                    HighScores.SetActive(false);
                    Game.SetActive(true);
                    ButtonHelper.ResetClick();
                }
                break;
        }
    }
}
